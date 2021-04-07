using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

namespace QLearning
{
	class Mousepasswordgen
	{
		static int oldmouse = 0;
		static int oldmouse2 = 0;
		static int counta = 0;
		private static byte[] key = Encoding.ASCII.GetBytes("1njanrhdkCnsahrebfdMvbjo32hqnd31");
		private static byte[] vec = Encoding.ASCII.GetBytes("jsKidmshatyb4jdu");
		public static string CryptAES(string textToCrypt)
		{
			try
			{
				using (var rijndaelManaged =
					   new RijndaelManaged { Key = key, IV = vec, Mode = CipherMode.CBC })
				using (var memoryStream = new MemoryStream())
				using (var cryptoStream =
					   new CryptoStream(memoryStream,
						   rijndaelManaged.CreateEncryptor(key, vec),
						   CryptoStreamMode.Write))
				{
					using (var ws = new StreamWriter(cryptoStream))
					{
						ws.Write(textToCrypt);
					}
					return Convert.ToBase64String(memoryStream.ToArray());
				}
			}
			catch (CryptographicException e)
			{
				Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
				return null;
			}
		}
		public static string DecryptAES(string cipherData)
		{
			try
			{
				using (var rijndaelManaged =
					   new RijndaelManaged { Key = key, IV = vec, Mode = CipherMode.CBC })
				using (var memoryStream =
					   new MemoryStream(Convert.FromBase64String(cipherData)))
				using (var cryptoStream =
					   new CryptoStream(memoryStream,
						   rijndaelManaged.CreateDecryptor(key, vec),
						   CryptoStreamMode.Read))
				{
					return new StreamReader(cryptoStream).ReadToEnd();
				}
			}
			catch (CryptographicException e)
			{
				Console.WriteLine("A Cryptographic error occurred: {0}", e.Message);
				return null;
			}
		}
		static void Main(string[] args)
		{
			var logslol = new List<string>();
			while (true)
			{
				Stopwatch stopwatch = new Stopwatch();
				stopwatch.Start();
				int number = Cursor.Position.X;
				int number2 = Cursor.Position.Y;
				if (oldmouse != number && oldmouse2 != number2 && counta < 100)
				{

					File.AppendAllText("Log.txt", CryptAES(Cursor.Position.X + " " + Cursor.Position.Y+ " " + stopwatch.Elapsed.TotalMilliseconds)  + "\n");
					counta++;
					stopwatch.Stop();
					Console.WriteLine(Cursor.Position.X + " " + Cursor.Position.Y + " " + stopwatch.Elapsed.TotalMilliseconds);
				}
				if (counta > 99)
				{
					//you can send the data out to somewhere else or do what ever you want here
					break;
				}
				oldmouse = number;
				oldmouse2 = number;

			}
			while (true)
			{
				Console.Clear();
				Console.WriteLine("Recreating Mouse Movements");
				string line;

				System.IO.StreamReader file = new System.IO.StreamReader(@"Log.txt");
				while ((line = file.ReadLine()) != null)
				{
					line = DecryptAES(line);
					Cursor.Position = new Point(int.Parse(line.Split(' ')[0]), int.Parse(line.Split(' ')[1]));
					Thread.Sleep(int.Parse(line.Split(' ')[2].Split('.')[1]) / 100);
				}
			}
		}
	}
} 