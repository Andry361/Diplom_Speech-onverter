using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;

namespace Diploma_SpeechСonverter.ProcessorWav
{

    [StructLayout(LayoutKind.Sequential)]
    // Структура, описывающая заголовок WAV файла.
    internal class WavHeader
    {
        // WAV-формат начинается с RIFF-заголовка:

        // Содержит символы "RIFF" в ASCII кодировке
        // (0x52494646 в big-endian представлении)
        public UInt32 ChunkId;

        // 36 + subchunk2Size, или более точно:
        // 4 + (8 + subchunk1Size) + (8 + subchunk2Size)
        // Это оставшийся размер цепочки, начиная с этой позиции.
        // Иначе говоря, это размер файла - 8, то есть,
        // исключены поля chunkId и chunkSize.
        public UInt32 ChunkSize;

        // Содержит символы "WAVE"
        // (0x57415645 в big-endian представлении)
        public UInt32 Format;

        // Формат "WAVE" состоит из двух подцепочек: "fmt " и "data":
        // Подцепочка "fmt " описывает формат звуковых данных:

        // Содержит символы "fmt "
        // (0x666d7420 в big-endian представлении)
        public UInt32 Subchunk1Id;

        // 16 для формата PCM.
        // Это оставшийся размер подцепочки, начиная с этой позиции.
        public UInt32 Subchunk1Size;

        // Аудио формат, полный список можно получить здесь http://audiocoding.ru/wav_formats.txt
        // Для PCM = 1 (то есть, Линейное квантование).
        // Значения, отличающиеся от 1, обозначают некоторый формат сжатия.
        public UInt16 AudioFormat;

        // Количество каналов. Моно = 1, Стерео = 2 и т.д.
        public UInt16 NumChannels;

        // Частота дискретизации. 8000 Гц, 44100 Гц и т.д.
        public UInt32 SampleRate;

        // sampleRate * numChannels * bitsPerSample/8
        public UInt32 ByteRate;

        // numChannels * bitsPerSample/8
        // Количество байт для одного сэмпла, включая все каналы.
        public UInt16 BlockAlign;

        // Так называемая "глубиная" или точность звучания. 8 бит, 16 бит и т.д.
        public UInt16 BitsPerSample;

        // Подцепочка "data" содержит аудио-данные и их размер.

        // Содержит символы "data"
        // (0x64617461 в big-endian представлении)
        public UInt32 Subchunk2Id;

        // numSamples * numChannels * bitsPerSample/8
        // Количество байт в области данных.
        public UInt32 Subchunk2Size;

        // Далее следуют непосредственно Wav данные.
    }
    class ReadWav
    {
        private WavHeader _header;
        public ReadWav()
        { }
        public short[] StartReadWav(string WavFileName)
        {
            _header = new WavHeader();
            // Размер заголовка
            var headerSize = Marshal.SizeOf(_header);

            var fileStream = new FileStream(WavFileName, FileMode.Open, FileAccess.Read);
            var buffer = new byte[headerSize];
            fileStream.Read(buffer, 0, headerSize);
            // Чтобы не считывать каждое значение заголовка по отдельности,
            // воспользуемся выделением unmanaged блока памяти
            var headerPtr = Marshal.AllocHGlobal(headerSize);
            // Копируем считанные байты из файла в выделенный блок памяти
            Marshal.Copy(buffer, 0, headerPtr, headerSize);
            // Преобразовываем указатель на блок памяти к нашей структуре
            Marshal.PtrToStructure(headerPtr, _header);
            //Выдеение блока памяти под данные файла.
            int Subchun2SizeToInt = (int)_header.Subchunk2Size;
            var _data = new byte[Subchun2SizeToInt];
            fileStream.Read(_data, 0, Subchun2SizeToInt);
            var data = new short[Subchun2SizeToInt / 2];
          

            for (int i = 0, j = 0; i < Subchun2SizeToInt / 2; i++, j += 2)
            {
                byte[] twoBits = new byte[2];
                twoBits[0] = _data[j];
                twoBits[1] = _data[j + 1];
                data[i] = System.BitConverter.ToInt16(twoBits, 0);
            }

            fileStream.Close();

            return data;

        }

        public void writeWavFile(string WavFileName, bool pOverwite, short[] data)
        {
            var header = new WavHeader();
            FileStream fileStream;
            if (pOverwite)
                fileStream = File.Open(WavFileName, FileMode.Create);
            else
                fileStream = File.Open(WavFileName, FileMode.CreateNew);

            header.ChunkId = _header.ChunkId;
            header.ChunkSize = _header.ChunkSize;
            header.Format = _header.Format;
            header.Subchunk1Id = _header.Subchunk1Id;
            header.Subchunk1Size = _header.Subchunk1Size;
            header.AudioFormat = _header.AudioFormat;
            header.NumChannels = _header.NumChannels;
            header.SampleRate = _header.SampleRate;
            header.ByteRate = _header.ByteRate;
            header.BlockAlign = _header.BlockAlign;
            header.BitsPerSample = _header.BitsPerSample;
            header.Subchunk2Id = _header.Subchunk2Id;
            header.Subchunk2Size = (uint)data.Length*2;



            int headerSize = Marshal.SizeOf(header);            
            IntPtr buffer = Marshal.AllocHGlobal(headerSize);
            Marshal.StructureToPtr(header, buffer, false);            
            byte[] rawdatas = new byte[headerSize];           
            Marshal.Copy(buffer, rawdatas, 0, headerSize);
            Marshal.FreeHGlobal(buffer);
            
            byte[] data_byte = new byte[data.Length * 2];
            for (int i = 0, j = 0; i < data.Length; i++, j += 2)
            {
                byte[] arr = BitConverter.GetBytes(data[i]);
                data_byte[j] = arr[0];
                data_byte[j + 1] = arr[1];

            }    

            fileStream.Write(rawdatas, 0, headerSize);
            fileStream.Write(data_byte, 0, data.Length * 2);
            fileStream.Close();
        }
    }
}




