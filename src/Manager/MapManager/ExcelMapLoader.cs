using Mines.Defines;
using NPOI.SS.UserModel;
using PlayerInterface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mines.Manager.MapManager
{
    class ExcelMapLoader
    {
        public static Dictionary<string, BlockType[]> LoadExcelMaps(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            var ret = new Dictionary<string, BlockType[]>();

            foreach (var file in new DirectoryInfo(path).GetFiles())
            {
                if (file.Name.StartsWith("~$")) continue;

                try
                {
                    var wb = WorkbookFactory.Create(new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                    var nos = wb.NumberOfSheets;

                    for (int i = 0; i < nos; i++)
                    {
                        var result = ExcelMapLoader.LoadMapFromSheet(wb.GetSheetAt(i), Config.BlockCol, Config.BlockRow);
                        if(result != null) ret[file.Name + "-" + wb.GetSheetName(i)] = result;
                    }
                        
                }
                catch (Exception e)
                {
                    MessageBox.Show("[ExcelMapLoader] error from : " + file.FullName + " " + e.Message);
                }
            }

            return ret;
        }

        private static BlockType[] LoadMapFromSheet(ISheet s, int noc, int nor)
        {
            if (s.LastRowNum == 0) return null;
            if (s.LastRowNum != nor - 1)
                throw new Exception("sheet : " + s.SheetName + " error : number of row have to be " + nor);

            var ret = new BlockType[noc * nor];
            
            for (int i = 0; i < nor; i++)
            {
                var row = s.GetRow(i);

                if(row.LastCellNum != noc)
                    throw new Exception("sheet : " + s.SheetName + " error : number of col have to be " + noc);

                int test = 0;
                for (int j = 0; j < noc; j++)
                {
                    test++;
                    try
                    {
                        ret[(i * noc) + j] = (BlockType)row.GetCell(j).NumericCellValue;
                    }
                    catch
                    {
                        throw new Exception("sheet : " + s.SheetName + " error : unknown block type. row : " + i + " col : " + j);
                    }
                }
            }
            
            return ret;
        }
    }
}
