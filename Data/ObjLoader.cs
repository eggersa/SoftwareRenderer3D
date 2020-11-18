using Sr3D.SrMath;
using System;
using System.Globalization;
using System.IO;

namespace SoftwareRenderer3D.Data
{
    // https://en.wikipedia.org/wiki/Wavefront_.obj_file
    public static class ObjLoader
    {
        public static IndexedFaceSet Load(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"The file does not exist", path);
            }

            var faceSet = new IndexedFaceSet();

            var lineIndex = 1;
            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                string[] cols;
                if (string.IsNullOrWhiteSpace(line) || (cols = line.Split(' ', StringSplitOptions.RemoveEmptyEntries)).Length == 0)
                {
                    continue;
                }

                try
                {
                    if (cols[0] == "v")
                    {
                        var vertex = new Vector3(
                            float.Parse(cols[1], CultureInfo.InvariantCulture),
                            float.Parse(cols[2], CultureInfo.InvariantCulture),
                            float.Parse(cols[3], CultureInfo.InvariantCulture));
                        faceSet.Vertices.Add(vertex);

                        System.Diagnostics.Debug.WriteLine(vertex.ToString());
                    }
                    else if (cols[0] == "f")
                    {
                        // we use zero based index
                        faceSet.Indices.Add(int.Parse(cols[1]) - 1);
                        faceSet.Indices.Add(int.Parse(cols[2]) - 1);
                        faceSet.Indices.Add(int.Parse(cols[3]) - 1);
                    }
                }
                catch (Exception ex)
                {
                    throw new InvalidDataException($"Error in line {lineIndex}", ex);
                }

                lineIndex++;
            }

            return faceSet;
        }
    }
}
