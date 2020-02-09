using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Kociemba
{
    public class Tools
    {
        // ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        // Check if the cube string s represents a solvable cube.
        // 0: Cube is solvable
        // -1: There is not exactly one facelet of each colour
        // -2: Not all 12 edges exist exactly once
        // -3: Flip error: One edge has to be flipped
        // -4: Not all corners exist exactly once
        // -5: Twist error: One corner has to be twisted
        // -6: Parity error: Two corners or two edges have to be exchanged
        // 
        /// <summary>
        /// Check if the cube definition string s represents a solvable cube.
        /// </summary>
        /// <param name="s"> is the cube definition string , see <seealso cref="Facelet"/> </param>
        /// <returns> 0: Cube is solvable<br>
        ///         -1: There is not exactly one facelet of each colour<br>
        ///         -2: Not all 12 edges exist exactly once<br>
        ///         -3: Flip error: One edge has to be flipped<br>
        ///         -4: Not all 8 corners exist exactly once<br>
        ///         -5: Twist error: One corner has to be twisted<br>
        ///         -6: Parity error: Two corners or two edges have to be exchanged </returns>
        public static int verify(string s)
        {
            int[] count = new int[6];
            try
            {
                for (int i = 0; i < 54; i++)
                {
                    count[(int)CubeColor.Parse(typeof(CubeColor), i.ToString())]++;
                }
            }
            catch (Exception)
            {
                return -1;
            }

            for (int i = 0; i < 6; i++)
            {
                if (count[i] != 9)
                {
                    return -1;
                }
            }

            FaceCube fc = new FaceCube(s);
            CubieCube cc = fc.toCubieCube();

            return cc.verify();
        }

        /// <summary>
        /// Generates a random cube. </summary>
        /// <returns> A random cube in the string representation. Each cube of the cube space has the same probability. </returns>
        public static string randomCube()
        {
            CubieCube cc = new CubieCube();
            Random gen = new Random();
            cc.setFlip((short)gen.Next(CoordCube.N_FLIP));
            cc.setTwist((short)gen.Next(CoordCube.N_TWIST));
            do
            {
                cc.setURFtoDLB(gen.Next(CoordCube.N_URFtoDLB));
                cc.setURtoBR(gen.Next(CoordCube.N_URtoBR));
            } while ((cc.edgeParity() ^ cc.cornerParity()) != 0);
            FaceCube fc = cc.toFaceCube();
            return fc.to_fc_String();
        }


        // https://stackoverflow.com/questions/7742519/c-sharp-export-write-multidimension-array-to-file-csv-or-whatever
        // Kristian Fenn: https://stackoverflow.com/users/989539/kristian-fenn

        public static void SerializeTable(string filename, short[,] array)
        {
            EnsureFolder("Assets\\Kociemba\\Tables\\");
            BinaryFormatter bf = new BinaryFormatter();
            Stream s = File.Open("Assets\\Kociemba\\Tables\\" + filename, FileMode.Create);
            bf.Serialize(s, array);
            s.Close();
        }

        public static short[,] DeserializeTable(string filename)
        {
            EnsureFolder("Assets\\Kociemba\\Tables\\");
            Stream s = File.Open("Assets\\Kociemba\\Tables\\" + filename, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            short[,] array = (short[,])bf.Deserialize(s);
            s.Close();
            return array;
        }

        public static void SerializeSbyteArray(string filename, sbyte[] array)
        {
            EnsureFolder("Assets\\Kociemba\\Tables\\");
            BinaryFormatter bf = new BinaryFormatter();
            Stream s = File.Open("Assets\\Kociemba\\Tables\\" + filename, FileMode.Create);
            bf.Serialize(s, array);
            s.Close();
        }

        public static sbyte[] DeserializeSbyteArray(string filename)
        {
            EnsureFolder("Assets\\Kociemba\\Tables\\");
            Stream s = File.Open("Assets\\Kociemba\\Tables\\" + filename, FileMode.Open);
            BinaryFormatter bf = new BinaryFormatter();
            sbyte[] array = (sbyte[])bf.Deserialize(s);
            s.Close();
            return array;
        }

        // https://stackoverflow.com/questions/3695163/filestream-and-creating-folders
        // Joe: https://stackoverflow.com/users/13087/joe

        static void EnsureFolder(string path)
        {
            string directoryName = Path.GetDirectoryName(path);
            // If path is a file name only, directory name will be an empty string
            if (directoryName.Length > 0)
            {
                // Create all directories on the path that don't already exist
                Directory.CreateDirectory(directoryName);
            }
        }
    }    
}
