using System;
using System.Data.Odbc;

namespace cs_con_odbc_excel_select
{
    class Program
    {
        static void Main(string[] args)
        {
            OdbcConnection myConAccess;
            OdbcCommand myCommand;
            OdbcDataReader myReader;

            string dbpath = @"C:\app\workspace\販売管理.xls";

            // *************************************
            // System.Data.Odbc
            // *************************************
            myConAccess = new OdbcConnection();
            myConAccess.ConnectionString =
                "Provider=MSDASQL;DSN=H-XLSX";
                // $"Provider=MSDASQL;Driver={{Microsoft Excel Driver (*.xls, *.xlsx, *.xlsm, *.xlsb)}};dbq={dbpath};";
                // $"Provider=MSDASQL;DSN=H-XLSX;DATABASE={dbpath}";

            // 接続を開く
            try
            {
                myConAccess.Open();

                string myQuery = "SELECT * from 社員マスタ";

                using (myCommand = new OdbcCommand()) {
                    // 実行する為に必要な情報をセット
                    myCommand.CommandText = myQuery;
                    myCommand.Connection = myConAccess;

                    using (myReader = myCommand.ExecuteReader()) {

                        // 読み出し

                        while (myReader.Read()) {
                            // 文字列
                            Console.Write(GetValue(myReader, "社員コード") + " : ");
                            Console.Write(GetValue(myReader, "氏名") + " : ");
                            Console.Write(GetValue(myReader, "フリガナ") + " : ");
                            // 整数
                            Console.Write(GetValue(myReader, "給与") + " : ");
                            Console.Write(GetValue(myReader, "手当") + " : ");
                            // 日付
                            Console.Write(GetValue(myReader, "作成日") + " : ");
                            Console.Write(GetValue(myReader, "更新日"));

                            Console.WriteLine();

                        }

                        myReader.Close();
                    }

                }

                myConAccess.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("接続エラーです:" + ex.Message);
            }

            Console.WriteLine("処理が終了しました : Enter キーを入力してください");
            Console.ReadLine();

        }
        private static string GetValue(OdbcDataReader myReader, string fld_name) {

            string ret = "";
            int fld_no = 0;

            // 指定された列名より、テーブル内での定義順序番号を取得
            fld_no = myReader.GetOrdinal(fld_name);
            // 定義順序番号より、NULL かどうかをチェック
            if (myReader.IsDBNull(fld_no)) {
                ret = "";
            }
            else {
                // NULL でなければ内容をオブジェクトとして取りだして文字列化する
                ret = myReader.GetValue(fld_no).ToString();
            }

            // 列の値を返す
            return ret;
        }
    }
}
