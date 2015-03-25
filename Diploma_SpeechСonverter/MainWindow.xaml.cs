using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Research.DynamicDataDisplay.DataSources;
using Microsoft.Research.DynamicDataDisplay;
using System.Collections.ObjectModel;
using NAudio.Wave;
using Diploma_SpeechСonverter.Models;
using System.ComponentModel;
using Microsoft.Win32;
namespace SpeechСonverter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        #region Константы
        private const int N = 1024;
        private double PI = 3.14159265358979323846;
        #endregion
        #region NAudio filds
        private NAudio.Wave.WaveFileReader wave = null;
        private NAudio.Wave.DirectSoundOut outPut = null;

        NAudio.Wave.WaveIn sourceStream = null;
        NAudio.Wave.DirectSoundOut waveOut = null;
        NAudio.Wave.WaveFileWriter waveWriter = null;
        #endregion

        private readonly MainWindowViewModel _viewModel;// убрать readonly, прога должна чекать, если пользователь подключил микрофон

    
        private bool _shutdown;
        private short[] data;
        private Nullable<bool> resultOpenDialog;
        private Microsoft.Win32.OpenFileDialog dlg;
        private Diploma_SpeechСonverter.ProcessorWav.ReadWav file;
        public MainWindow()
        {
            _viewModel = new MainWindowViewModel();
            DataContext = _viewModel;
            InitializeComponent();            
        }
        
        #region Слой с настройками приложения

        private async void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            this.ToggleFlyout(0);
        }
        private void ToggleFlyout(int index)
        {
            var flyout = this.Flyouts.Items[index] as Flyout;
            if (flyout == null)
            {
                return;
            }

            flyout.IsOpen = !flyout.IsOpen;
        }
        #endregion
        #region NAudio method
        private void DisposeWave()
        {
            if (outPut != null)
            {
                if (outPut.PlaybackState == NAudio.Wave.PlaybackState.Playing) outPut.Stop();
                outPut.Dispose();
                outPut = null;
            }
            if (wave != null)
            {
                wave.Dispose();
                wave = null;
            }
        }
         private void Restart()
        {
            DisposeWave();

            wave = new NAudio.Wave.WaveFileReader(dlg.FileName);
            outPut = new NAudio.Wave.DirectSoundOut();
            outPut.Init(new NAudio.Wave.WaveChannel32(wave));
            outPut.Play();
        }


        //TODO: Сделать одну кнопку как play/pause  с меняющейся иконкой.
         private async void PlayWaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (outPut != null)
            {
                if (outPut.PlaybackState == NAudio.Wave.PlaybackState.Playing) Restart();
                else if (outPut.PlaybackState == NAudio.Wave.PlaybackState.Paused) Restart();// или Play()
                else outPut.Play();
            }
            else
            {
                ShowMessageDialog("Внимание!", "Перед прослушивание музыкальной композиции необходимо загрузить wav фаил.");
            }
        }

        private async void PauseWaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (outPut != null)
            {
                if (outPut.PlaybackState == NAudio.Wave.PlaybackState.Playing) outPut.Pause();
                else if (outPut.PlaybackState == NAudio.Wave.PlaybackState.Paused) outPut.Play();
            }
            else
            {
                (sender as System.Windows.Controls.Primitives.ToggleButton).IsChecked = false;
                ShowMessageDialog("Внимание!", "Перед прослушивание музыкальной композиции необходимо загрузить wav фаил.");
            }
        }
        #endregion

        private async void MetroWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _shutdown = false;
            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Quit",
                NegativeButtonText = "Cancel",
                AnimateShow = true,
                AnimateHide = false
            };

            RecodingStop();
            if (waveWriter != null)
            {
                waveWriter.Dispose();
                waveWriter = null;
            }


            var result = await this.ShowMessageAsync("Quit application?", "Sure you want to quit application?",
                MessageDialogStyle.AffirmativeAndNegative, mySettings);

            _shutdown = result == MessageDialogResult.Affirmative;

            if (_shutdown)
                Application.Current.Shutdown();
        }

        private void OpenFileTile_Click(object sender, RoutedEventArgs e)
        {
            dlg = new Microsoft.Win32.OpenFileDialog();

            dlg.DefaultExt = ".txt";
            dlg.Filter = "wav файлы (*.wav)|*.wav";

            resultOpenDialog = dlg.ShowDialog();

            if (resultOpenDialog != false)
            {
                file = new Diploma_SpeechСonverter.ProcessorWav.ReadWav();
                data = file.StartReadWav(dlg.FileName);
                (this.Flyouts.Items[0] as Flyout).IsOpen = false;

                DisposeWave();

                wave = new NAudio.Wave.WaveFileReader(dlg.FileName);
                outPut = new NAudio.Wave.DirectSoundOut();//new NAudio.Wave.WaveOut();
                outPut.Init(new NAudio.Wave.WaveChannel32(wave));
            }
        }


        private void BildOscillogram_Click(object sender, RoutedEventArgs e)
        {
            if (resultOpenDialog == true)
            {
                foreach (var graph in plotter.Children.ToArray())
                {
                    if (graph is LineGraph || graph is ElementMarkerPointsGraph)
                    {
                        ClearLines();
                    }
                }

                double step = 1.0;
                var x = Enumerable.Range(0, data.Length).Select(i => i * step).ToArray();
                var y = x.Select(v => data[(int)(v / step)] * 1.0).ToArray();

                var xDataSource = x.AsXDataSource();
                var yDataSource = y.AsYDataSource();
                CompositeDataSource compositeDataSource = xDataSource.Join(yDataSource);
                plotter.AddLineGraph(compositeDataSource, Colors.Goldenrod, 3, "Sine");
                plotter.FitToView();
            }
            else
            {
                ShowMessageDialog("Внимание!", "Перед построением осциллограммы необходимо загрузить wav фаил.");
            }
        }

        public void ClearLines()
        {
            var lgc = new Collection<IPlotterElement>();
            foreach (var x in plotter.Children)
            {
                if (x is LineGraph || x is ElementMarkerPointsGraph)
                    lgc.Add(x);
            }

            foreach (var x in lgc)
            {
                plotter.Children.Remove(x);
            }
        }





        private async void ShowMessageDialog(string title, string message)
        {
            // This demo runs on .Net 4.0, but we're using the Microsoft.Bcl.Async package so we have async/await support
            // The package is only used by the demo and not a dependency of the library!
            MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;

            var mySettings = new MetroDialogSettings()
            {
                AffirmativeButtonText = "Ok",
                ColorScheme = MetroDialogColorScheme.Theme
            };

            MessageDialogResult result = await this.ShowMessageAsync(title, message,
                MessageDialogStyle.Affirmative, mySettings);
        }

        private void OpenMenuMusicManagement_Click(object sender, RoutedEventArgs e)
        {
            this.ToggleFlyout(1);
        }

        private void WaveRecodingButton_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as System.Windows.Controls.Primitives.ToggleButton).IsChecked == true)
            {
                if (_viewModel.Microphones.Count == 0)
                    ShowMessageDialog("Извините :(", "Запись не может начаться, так как ни один микрофон не подключен.");

                int deviceNumber = ((Microphone)ListMicrophones.SelectedItems[0]).MicrophoneId - 1;// Вычитаем -1, так как при добавлении в Microphones мы прибавляли 1, чтобы отображение в ListView не начиналась с 0

                sourceStream = new NAudio.Wave.WaveIn();
                sourceStream.DeviceNumber = deviceNumber;
                sourceStream.WaveFormat = new NAudio.Wave.WaveFormat(8000/*44100*/, NAudio.Wave.WaveIn.GetCapabilities(deviceNumber).Channels);

                NAudio.Wave.WaveInProvider waveIn = new NAudio.Wave.WaveInProvider(sourceStream);

                sourceStream.DataAvailable += new EventHandler<NAudio.Wave.WaveInEventArgs>(sourceStream_DataAvailable);



                waveOut = new NAudio.Wave.DirectSoundOut();
                waveOut.Init(waveIn);

                sourceStream.StartRecording();
                waveOut.Play();
            }
            else RecodingStop();
            
        }

        private void RecodingStop()
        {
            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }
            if (sourceStream != null)
            {
                sourceStream.StopRecording();
                sourceStream.Dispose();
                sourceStream = null;
            }
        }

        private void SaveWaveFileButton_Click(object sender, RoutedEventArgs e)
        {
            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
                waveOut = null;
            }
            if (sourceStream != null)
            {
                sourceStream.StopRecording();
            }
            WaveRecodingButton.IsChecked = false;

            if (_viewModel.Microphones.Count == 0)
                ShowMessageDialog("Извините :(", "Файл не может быть сохранен, так как потеряно соеденение с микрофоном.");


            if (sourceStream == null)
            {
                ShowMessageDialog("Извините :(", "Файл не может быть сохранен, так как вы ничего не записали.");
                return;
            }

            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Wave File (*.wav)|*.wav;";
            bool? IsSaveOk = save.ShowDialog();
            if (IsSaveOk != true) return;


          
            waveWriter = new NAudio.Wave.WaveFileWriter(save.FileName, sourceStream.WaveFormat);
           
            
            for (int i = 0; i < buffer.Count; i++)
			{
                waveWriter.WriteData(buffer[i], 0, BytesRecorded[i]);
                waveWriter.Flush();
			}
                         

            if (waveWriter != null)
            {
                waveWriter.Dispose();
                waveWriter = null;
                this.ToggleFlyout(1);
            }
            ShowMessageDialog("Ура :)", "Файл успешно сохранен.");
        }
        static List<byte[]> buffer = new List<byte[]>();
        static List<int> BytesRecorded = new List<int>();
        private void sourceStream_DataAvailable(object sender, NAudio.Wave.WaveInEventArgs e)
        {
            byte[] mas = new byte[e.BytesRecorded];
            Array.Copy(e.Buffer, mas, e.BytesRecorded);
            buffer.Add(mas);
            BytesRecorded.Add(e.BytesRecorded);
            //if (waveWriter == null) return;

            //waveWriter.WriteData(e.Buffer, 0, e.BytesRecorded);
            //waveWriter.Flush();
        }
    }

    public class MainWindowViewModel 
    {
        public List<Microphone> Microphones { get; set; }

        public MainWindowViewModel()
        {


            Microphones = new List<Microphone>();
            List<NAudio.Wave.WaveInCapabilities> sources = new List<NAudio.Wave.WaveInCapabilities>();

            for (int i = 0; i < NAudio.Wave.WaveIn.DeviceCount; i++)
            {
                sources.Add(NAudio.Wave.WaveIn.GetCapabilities(i));
            }

           for (int i = 0; i < sources.Count; i++)
			{
			 Microphones.Add(new Microphone { MicrophoneId = i+1, NameMicrophone = sources[i].ProductName });
                //ListViewItem item = new ListViewItem(source.ProductName);
                //item.SubItems.Add(new ListViewItem.ListViewSubItem(item, source.Channels.ToString()));
                //sourceList.Items.Add(item);
			}
        }
    }
    
}
