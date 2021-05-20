using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuestionnaireSort.Model
{
    public class SortManager
    {
        public const string File1SortID = "工號";
        public const string File1SortDep = "部門名稱";
        public const string File1SortName = "姓名";

        public const string File2SortID = "工號";
        public const string File2SortDep = "部門";
        public const string File2SortName = "姓名";
        public SortManager()
        {
            File1Mapping = new Dictionary<string, int>();
            File1Datas = new List<File1Data>();

            File2Mapping = new Dictionary<string, int>();
            File2Datas = new List<File2Data>();

        }

        public string File2Fields { get; private set; }

        public Dictionary<string, int> File2Mapping { get; }

        public List<File2Data> File2Datas { get; }

        public string Selected2FileName { get; set; }

        public string IsOkSelected2_ErrMsg { get; private set; }

        public bool IsOkSelected2_File()
        {
            try
            {
                var reader = new StreamReader(File.OpenRead(Selected2FileName));
                var fieldLine = reader.ReadLine();
                var Fields = fieldLine.Split(new char[] { ',' }).Select((item) => item.Trim());
                reader.Close();
                reader.Dispose();

                if (!Fields.Contains(File2SortID))
                {
                    IsOkSelected2_ErrMsg = $"Not Found {File2SortID}";
                    return false;
                }

                if (!Fields.Contains(File2SortDep))
                {
                    IsOkSelected2_ErrMsg = $"Not Found {File2SortDep}";
                    return false;
                }
                if (!Fields.Contains(File2SortName))
                {
                    IsOkSelected2_ErrMsg = $"Not Found {File2SortName}";
                    return false;
                }
                IsOkSelected2_ErrMsg = "Check Done";
                return true;
            }
            catch(Exception ex)
            {
                IsOkSelected2_ErrMsg = ex.Message;
                return false;
            }
            
        }

        public void CollectFile2Datas()
        {
            var reader = new StreamReader(File.OpenRead(Selected2FileName));
            string fieldLine = reader.ReadLine();
            File2Fields = fieldLine;
            var Fields = fieldLine.Split(new char[] { ',' }).Select((item) => item.Trim()).ToList();

            //Mapping
            File2Mapping.Clear();
            File2Mapping.Add(File2SortID, Fields.IndexOf(File2SortID));
            File2Mapping.Add(File2SortDep, Fields.IndexOf(File2SortDep));
            File2Mapping.Add(File2SortName, Fields.IndexOf(File2SortName));

            File2Datas.Clear();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',').Select((item) => item.Trim()).ToList();
                var othersValue = values.Where(
                    (item) =>
                    File2Mapping[File2SortID] != values.IndexOf(item) &&
                    File2Mapping[File2SortDep] != values.IndexOf(item) &&
                    File2Mapping[File2SortName] != values.IndexOf(item));
                File2Data f2d = new File2Data
                    (
                    id: values[File2Mapping[File2SortID]],
                    name: values[File2Mapping[File2SortName]],
                    dep: values[File2Mapping[File2SortDep]],
                    others: string.Join(",", othersValue)
                    );
                File2Datas.Add(f2d);
            }

            reader.Close();
            reader.Dispose();
        }

        public string File1Fields { get; private set; }

        public Dictionary<string, int> File1Mapping { get; }

        public List<File1Data> File1Datas { get; }

        public string Selected1FileName { get; set; }

        public string IsOkSelected1_ErrMsg { get; private set; }

        public bool IsOkSelected1_File()
        {
            try
            {
                var reader = new StreamReader(File.OpenRead(Selected1FileName));
                var fieldLine = reader.ReadLine();
                var Fields = fieldLine.Split(new char[] { ',' }).Select((item) => item.Trim());

                reader.Close();
                reader.Dispose();

                if (!Fields.Contains(File1SortID))
                {
                    IsOkSelected1_ErrMsg = $"Not Found {File1SortID}";
                    return false;
                }

                if (!Fields.Contains(File1SortDep))
                {
                    IsOkSelected1_ErrMsg = $"Not Found {File1SortDep}";
                    return false;
                }
                if (!Fields.Contains(File1SortName))
                {
                    IsOkSelected1_ErrMsg = $"Not Found {File1SortName}";
                    return false;
                }
                IsOkSelected1_ErrMsg = "Check Done";
                return true;
            }
            catch(Exception ex)
            {
                IsOkSelected1_ErrMsg = ex.Message;
                return false;
            }
            
        }

        

        public void CollectFile1Datas()
        {
            var reader = new StreamReader(File.OpenRead(Selected1FileName));
            string fieldLine =reader.ReadLine();
            File1Fields = fieldLine;
            var Fields = fieldLine.Split(new char[] { ',' }).Select((item) => item.Trim()).ToList();

            //Mapping
            File1Mapping.Clear();
            File1Mapping.Add(File1SortID, Fields.IndexOf(File1SortID));
            File1Mapping.Add(File1SortDep, Fields.IndexOf(File1SortDep));
            File1Mapping.Add(File1SortName, Fields.IndexOf(File1SortName));

            File1Datas.Clear();
            while(!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                var values = line.Split(',').Select((item) => item.Trim()).ToList();
                File1Data f1d = new File1Data
                    (
                    id: values[File1Mapping[File1SortID]],
                    name: values[File1Mapping[File1SortName]],
                    dep: values[File1Mapping[File1SortDep]],
                    orig: line
                    );
                File1Datas.Add(f1d);
            }

            reader.Close();
            reader.Dispose();

        }

        public List<string> OkIDs { get; } = new List<string>();

        public List<string> ErrorIDs { get; } = new List<string>();

        public List<string> NotWriteIDs { get; } = new List<string>();

        public void CheckIDsIsAllFound()
        {
            //問卷Data
            var WrittenIDs = File2Datas.Select((data) => data.ID);
            //已填的ID 與 人事Data id 比對
            OkIDs.Clear();
            OkIDs.AddRange(File1Datas.Where((data) => WrittenIDs.Contains(data.ID)).Select((data) => data.ID));

            //已填的ID但未從人事資料中找到
            ErrorIDs.Clear();
            ErrorIDs.AddRange(WrittenIDs.Except(OkIDs));

            //尚未填寫的IDs
            NotWriteIDs.Clear();
            NotWriteIDs.AddRange(File1Datas.Where((data) => !OkIDs.Contains(data.ID)).Select(data => data.ID));
        }

        public List<File2Data> OKFile2Datas { get; } = new List<File2Data>();

        public void FixedOkIDsDep()
        {
            OKFile2Datas.Clear();
            OKFile2Datas.AddRange(File2Datas.Where((data) => OkIDs.Contains(data.ID)));
            foreach (var data in OKFile2Datas)
            {
                data.Dep = File1Datas.First((item) => item.ID == data.ID).Dep;
                data.Name = File1Datas.First((item) => item.ID == data.ID).Name;
            }
        }

        public void WriteOKFile2Datas_SortByDep(string FolderPath, string FileName)
        {
            string fileName = Path.Combine(FolderPath, FileName);
            OKFile2Datas.Sort((a, b) => a.Dep.CompareTo(b.Dep));
            using (var file = new StreamWriter(File.OpenWrite(fileName), Encoding.UTF8))
            {
                file.WriteLine(File2Fields);
                foreach (var item in OKFile2Datas)
                {
                    if(int.TryParse(item.ID, out int _id))
                        file.WriteLine(item.ToString());
                }
            }
        }

        public void WriteUnKnownIDs_SortByID(string FolderPath, string FileName)
        {
            string fileName = Path.Combine(FolderPath, FileName);
            var file2UnknownID_Datas = File2Datas.Where((data) => ErrorIDs.Contains(data.ID));
            file2UnknownID_Datas.ToList().Sort((a, b) => a.ID.CompareTo(b.ID));
            using (var file = new StreamWriter(File.OpenWrite(fileName), Encoding.UTF8))
            {
                file.WriteLine(File2Fields);
                foreach (var item in file2UnknownID_Datas)
                {
                    if (int.TryParse(item.ID, out int _id))
                        file.WriteLine(item.ToString());
                }
            }
        }

        public void WriteUnDoIDs_SortByDep(string FolderPath, string FileName)
        {
            string fileName = Path.Combine(FolderPath, FileName);
            var file1UnDoID_Datas = File1Datas.Where((data) => !OkIDs.Contains(data.ID));
            file1UnDoID_Datas.ToList().Sort((a, b) => a.Dep.CompareTo(b.Dep));
            using (var file = new StreamWriter(File.OpenWrite(fileName), Encoding.UTF8))
            {
                file.WriteLine(File1Fields);
                foreach (var item in file1UnDoID_Datas)
                {
                    if (int.TryParse(item.ID, out int _id))
                        file.WriteLine(item.OrignalRowData);
                }
            }
        }

        public void WriteFixedIDs(string FolderPath, string FileName)
        {
            string fileName = Path.Combine(FolderPath, FileName);
            var file2UnknownID_Datas = File2Datas.Where((data) => ErrorIDs.Contains(data.ID));
            file2UnknownID_Datas.ToList().Sort((a, b) => a.ID.CompareTo(b.ID));

            //fixed
            foreach (var item in file2UnknownID_Datas)
            {
                var data = File1Datas.FirstOrDefault((_data) => _data.Name == item.Name);
                if (data != null)
                {
                    item.ID = data.ID;
                    item.Dep = data.Dep;
                }
            }

            file2UnknownID_Datas.ToList().Sort((a, b) => a.Dep.CompareTo(b.Dep));

            using (var file = new StreamWriter(File.OpenWrite(fileName), Encoding.UTF8))
            {
                file.WriteLine(File2Fields);
                foreach (var item in file2UnknownID_Datas)
                {
                    var data = File1Datas.FirstOrDefault((_data) => _data.Name == item.Name);
                    if(data != null)
                    {
                        file.WriteLine(item.ToString());
                    }
                }
            }
        }

    }

    public class File1Data
    {
        public File1Data(string id, string name, string dep, string orig)
        {
            this.ID = id;
            this.Name = name;
            this.Dep = dep;
            this.OrignalRowData = orig;
        }
        public string ID { get; }
        public string Name { get; }
        public string Dep { get; }
        public string OrignalRowData { get; }
        public override string ToString()
        {
            return $"{ID},{Name},{Dep}";
        }
    }

    public class File2Data
    {
        public File2Data(string id, string name, string dep, string others)
        {
            this.ID = id;
            this.Name = name;
            this.Dep = dep;
            this.Others = others;
        }
        public string ID { get; set; }
        public string Name { get; set; }
        public string Dep { get; set; }
        public string Others { get; }
        public override string ToString()
        {
            return $"{Dep},{ID},{Name},{Others}";
        }

    }
}
