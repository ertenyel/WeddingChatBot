using System;
using System.Collections.Generic;
using System.IO;
using Telegram.Bot.Types;

namespace WeddingChatBot
{
    public class Media: IDisposable
    {
        public List<FileStream> Streams { get; set; }
        public List<InputMediaPhoto> GetAlbumPhotos(string url)
        {
            string[] urlArray = url.Split(';');
            List<string> album = new List<string>();

            foreach (string file in urlArray)
            {
                if (System.IO.File.Exists(file))
                    album.Add(file);
            }

            if (album.Count > 0)
            {
                Streams = new List<FileStream>();
                foreach (string file in album)
                    Streams.Add(System.IO.File.OpenRead(file));

                List<InputMediaPhoto> photos = new List<InputMediaPhoto>();

                for (int i = 0; i < Streams.Count; i++)
                    photos.Add(new InputMediaPhoto(new InputMedia(Streams[i], "Photo" + i)));

                return photos;
            }
            return null;
        }
        public void Dispose()
        {
            if (Streams != null)
            {
                foreach (FileStream stream in Streams)
                    stream.Close();
            }
        }
    }
}
