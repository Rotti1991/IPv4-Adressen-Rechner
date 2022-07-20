using System;
using System.Linq;
using System.Windows;

namespace IPv4_Adressen_Rechner
{

    public partial class MainWindow : Window
    {
        // Deklaration eines 2-Dimensionalen String Arrays
        private readonly string[,] arrays = new string[4, 8];

        public MainWindow()
        {
            InitializeComponent();
        }

        private void ReadValues()
        {
            arrays[0, 0] = Convert.ToString(IPNum1.Text);
            arrays[0, 1] = Convert.ToString(IPNum2.Text);
            arrays[0, 2] = Convert.ToString(IPNum3.Text);
            arrays[0, 3] = Convert.ToString(IPNum4.Text);
            arrays[1, 0] = Convert.ToString(SubNum1.Text);
            arrays[1, 1] = Convert.ToString(SubNum2.Text);
            arrays[1, 2] = Convert.ToString(SubNum3.Text);
            arrays[1, 3] = Convert.ToString(SubNum4.Text);

        }

        // Butten Ergebnis
        private void Result_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReadValues();
                NIDandFirstIp();
                BCandLastIp();
                HowManyIps();
            }
            catch (Exception)
            {
                _ = MessageBox.Show("Eingabefelder dürfen nicht leer sein!");
            }
        }

        // Button Löschen
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            reset();
        }

        // Funktion zur ermittlung der Netzwerk-ID und der erst möglichen IP Adresse
        private void NIDandFirstIp()
        {
            NIDIp1.Text = CompareNIDandFirst(arrays[0, 0], arrays[1, 0]);
            NIDIp2.Text = CompareNIDandFirst(arrays[0, 1], arrays[1, 1]);
            NIDIp3.Text = CompareNIDandFirst(arrays[0, 2], arrays[1, 2]);
            NIDIp4.Text = CompareNIDandFirst(arrays[0, 3], arrays[1, 3]);
            FirstIp1.Text = NIDIp1.Text;
            FirstIp2.Text = NIDIp2.Text;
            FirstIp3.Text = NIDIp3.Text;
            int temp = int.Parse(NIDIp4.Text) + 1;
            FirstIp4.Text = Convert.ToString(temp);
        }

        // Funktion zur ermittlung der letzt möglichen IP Adresse und des Broadcast
        private void BCandLastIp()
        {
            string binaryip1 = FormatNumbers(HexToBinary(IntToHex(int.Parse(arrays[0, 0]))));
            string binaryip2 = FormatNumbers(HexToBinary(IntToHex(int.Parse(arrays[0, 1]))));
            string binaryip3 = FormatNumbers(HexToBinary(IntToHex(int.Parse(arrays[0, 2]))));
            string binaryip4 = FormatNumbers(HexToBinary(IntToHex(int.Parse(arrays[0, 3]))));
            string binarysub1 = FormatNumbers(HexToBinary(IntToHex(int.Parse(arrays[1, 0]))));
            string binarysub2 = FormatNumbers(HexToBinary(IntToHex(int.Parse(arrays[1, 1]))));
            string binarysub3 = FormatNumbers(HexToBinary(IntToHex(int.Parse(arrays[1, 2]))));
            string binarysub4 = FormatNumbers(HexToBinary(IntToHex(int.Parse(arrays[1, 3]))));
            string temp1 = BinStringToDezString(binaryip1);
            string temp2 = BinStringToDezString(binaryip2);
            string temp3 = BinStringToDezString(binaryip3);
            string temp4 = BinStringToDezString(binaryip4);
            string temp5 = BinStringToDezString(LogicalNOT(binarysub1));
            string temp6 = BinStringToDezString(LogicalNOT(binarysub2));
            string temp7 = BinStringToDezString(LogicalNOT(binarysub3));
            string temp8 = BinStringToDezString(LogicalNOT(binarysub4));
            BCIp1.Text = LogicalBinLinkOr(temp1, temp5);
            BCIp2.Text = LogicalBinLinkOr(temp2, temp6);
            BCIp3.Text = LogicalBinLinkOr(temp3, temp7);
            BCIp4.Text = LogicalBinLinkOr(temp4, temp8);
            LastIp1.Text = BCIp1.Text;
            LastIp2.Text = BCIp2.Text;
            LastIp3.Text = BCIp3.Text;
            LastIp4.Text = Convert.ToString(int.Parse(BCIp4.Text) - 1);
        }

        // Funktion für die ermittlung der Netze und anazahl der maximal Verfügbaren IP Adressen
        private void HowManyIps()
        {
            string Sub4 = Convert.ToString(SubNum4.Text);
            string Sub3 = Convert.ToString(SubNum3.Text);
            string Sub2 = Convert.ToString(SubNum2.Text);
            string Sub1 = Convert.ToString(SubNum1.Text);

            if (Sub4 != "255" && Sub3 != "255" && Sub2 != "255" && Sub1 != "255")
            {
                int w = int.Parse(Sub4);
                int x = int.Parse(Sub3);
                int y = int.Parse(Sub2);
                int z = int.Parse(Sub1);
                string temp1 = HexToBinary(IntToHex(w));
                string temp2 = HexToBinary(IntToHex(x));
                string temp3 = HexToBinary(IntToHex(y));
                string temp4 = HexToBinary(IntToHex(z));
                arrays[2, 0] = FormatNumbers(temp1);
                arrays[2, 1] = FormatNumbers(temp2);
                arrays[2, 2] = FormatNumbers(temp3);
                arrays[2, 3] = FormatNumbers(temp4);
                arrays[3, 0] = HowMany0(arrays[2, 0]);
                arrays[3, 1] = HowMany1(arrays[2, 0]);
                arrays[3, 2] = HowMany0(arrays[2, 1]);
                arrays[3, 3] = HowMany1(arrays[2, 1]);
                arrays[3, 4] = HowMany0(arrays[2, 2]);
                arrays[3, 5] = HowMany1(arrays[2, 2]);
                arrays[3, 6] = HowMany0(arrays[2, 3]);
                arrays[3, 7] = HowMany1(arrays[2, 3]);
                int maxip = (int)Math.Pow(2, int.Parse(arrays[3, 0]) + int.Parse(arrays[3, 2]) + int.Parse(arrays[3, 4]) + int.Parse(arrays[3, 6]));
                MaxNets.Text = Convert.ToString((int)Math.Pow(2, int.Parse(arrays[3, 1]) + int.Parse(arrays[3, 3]) + int.Parse(arrays[3, 5]) + int.Parse(arrays[3, 7])));
                int used = int.Parse(Convert.ToString(Used.Text));
                int res = maxip - used - 2;
                MaxIps.Text = Convert.ToString(res);
            }

            else if (Sub4 != "255" && Sub3 != "255" && Sub2 != "255")
            {
                int w = int.Parse(Sub4);
                int x = int.Parse(Sub3);
                int y = int.Parse(Sub2);
                string temp1 = HexToBinary(IntToHex(w));
                string temp2 = HexToBinary(IntToHex(x));
                string temp3 = HexToBinary(IntToHex(y));
                arrays[2, 0] = FormatNumbers(temp1);
                arrays[2, 1] = FormatNumbers(temp2);
                arrays[2, 2] = FormatNumbers(temp3);
                arrays[3, 0] = HowMany0(arrays[2, 0]);
                arrays[3, 1] = HowMany1(arrays[2, 0]);
                arrays[3, 2] = HowMany0(arrays[2, 1]);
                arrays[3, 3] = HowMany1(arrays[2, 1]);
                arrays[3, 4] = HowMany0(arrays[2, 2]);
                arrays[3, 5] = HowMany1(arrays[2, 2]);
                int maxip = (int)Math.Pow(2, int.Parse(arrays[3, 0]) + int.Parse(arrays[3, 2]) + int.Parse(arrays[3, 4]));
                MaxNets.Text = Convert.ToString((int)Math.Pow(2, int.Parse(arrays[3, 1]) + int.Parse(arrays[3, 3]) + int.Parse(arrays[3, 5])));
                int used = int.Parse(Convert.ToString(Used.Text));
                int res = maxip - used - 2;
                MaxIps.Text = Convert.ToString(res);
            }

            else if (Sub4 != "255" && Sub3 != "255")
            {
                int w = int.Parse(Sub4);
                int x = int.Parse(Sub3);
                string temp3 = HexToBinary(IntToHex(w));
                string temp4 = HexToBinary(IntToHex(x));
                arrays[2, 0] = FormatNumbers(temp3);
                arrays[2, 1] = FormatNumbers(temp4);
                arrays[3, 0] = HowMany0(arrays[2, 0]);
                arrays[3, 1] = HowMany1(arrays[2, 0]);
                arrays[3, 2] = HowMany0(arrays[2, 1]);
                arrays[3, 3] = HowMany1(arrays[2, 1]);
                int maxip = (int)Math.Pow(2, int.Parse(arrays[3, 0]) + int.Parse(arrays[3, 2]));
                MaxNets.Text = Convert.ToString((int)Math.Pow(2, int.Parse(arrays[3, 1]) + int.Parse(arrays[3, 3])));
                int used = int.Parse(Convert.ToString(Used.Text));
                int res = maxip - used - 2;
                MaxIps.Text = Convert.ToString(res);
            }

            else if (Sub4 != "255")
            {
                int w = int.Parse(Sub4);
                string temp2 = HexToBinary(IntToHex(w));
                arrays[2, 0] = FormatNumbers(temp2);
                arrays[3, 0] = HowMany0(arrays[2, 0]);
                arrays[3, 1] = HowMany1(arrays[2, 0]);
                int max = (int)Math.Pow(2, int.Parse(arrays[3, 0]));
                int max2 = (int)Math.Pow(2, int.Parse(arrays[3, 1]));
                int used = int.Parse(Convert.ToString(Used.Text));
                int res = max - used - 2;
                MaxIps.Text = Convert.ToString(res);
                MaxNets.Text = Convert.ToString(max2);
            }
        }

        // Blockweiser Logischer UND vergleich zwischen dem jeweiligen Block der IP und passenden Block der Subnetzmaske
        private string CompareNIDandFirst(string val1, string val2)
        {
            int x1 = Convert.ToInt32(val1);
            int y1 = Convert.ToInt32(val2);
            string newIP = Convert.ToString(LogicalBinLinkAnd(x1, y1));
            return newIP;
        }
        // Konvertiert Hexadezimal zu Binär

        private string HexToBinary(string hexValue)

        {
            string binary = Convert.ToString(Convert.ToInt32(hexValue, 16), 2);
            return binary;
        }

        // Funktion für Logische UND verknüpfung
        private int LogicalBinLinkAnd(int x, int y)
        {
            int linked = (byte)(x & y);
            return linked;
        }

        // Funktion für Logische ODER verknüpfung
        private string LogicalBinLinkOr(string bin1, string bin2)
        {
            int x = int.Parse(bin1);
            int y = int.Parse(bin2);
            int linked = x | y;
            string result = Convert.ToString(linked);
            return result;
        }

        // Funktion Logische negierung
        private string LogicalNOT(string str1)
        {
            string str2 = "";
            for (int n = 0; n < str1.Length; ++n)
            {
                str2 += (~(str1[n] - '0')) & 0x01;
            }
            return str2;
        }

        // Funktion um führenden nullen hinzuzufügen 
        public string FormatNumbers(string x)
        {
            x = x.PadLeft(8, '0');
            return x;
        }

        // Funktion um Binär Strings zu Dezimal Strings zu wandeln
        private string BinStringToDezString(string bin)
        {
            int x = Convert.ToInt32(bin, 2);
            string result = Convert.ToString(x);
            return result;
        }

        // Funktion um die Anzahl der nullen einer Binärzahl zu ermitteln
        private string HowMany0(string x)
        {
            char ch0 = '0';
            int how0 = x.Count(f => f == ch0);
            string y = how0.ToString();
            return y;
        }

        // Funktion um die Anzahl der einsen einer Binärzahl zu ermitteln
        private string HowMany1(string x)
        {
            char ch1 = '1';
            int how1 = x.Count(f => f == ch1);
            string y = how1.ToString();
            return y;
        }

        // Funktion um Integer werte in Hexadezimal String zu wandeln
        private string IntToHex(int x)
        {
            string hexValue = x.ToString("X");
            return hexValue;
        }

        // Setzt alle eingegebenen und ermittelten werte zurück
        private void reset()
        {
            IPNum1.Text = null;
            IPNum2.Text = null;
            IPNum3.Text = null;
            IPNum4.Text = null;
            SubNum1.Text = null;
            SubNum2.Text = null;
            SubNum3.Text = null;
            SubNum4.Text = null;
            NIDIp1.Text = null;
            NIDIp2.Text = null;
            NIDIp3.Text = null;
            NIDIp4.Text = null;
            FirstIp1.Text = null;
            FirstIp2.Text = null;
            FirstIp3.Text = null;
            FirstIp4.Text = null;
            LastIp1.Text = null;
            LastIp2.Text = null;
            LastIp3.Text = null;
            LastIp4.Text = null;
            BCIp1.Text = null;
            BCIp2.Text = null;
            BCIp3.Text = null;
            BCIp4.Text = null;
            MaxIps.Text = null;
            MaxNets.Text = null;
            arrays[0, 0] = null;
            arrays[0, 1] = null;
            arrays[0, 2] = null;
            arrays[0, 3] = null;
            arrays[1, 0] = null;
            arrays[1, 1] = null;
            arrays[1, 2] = null;
            arrays[1, 3] = null;
            arrays[2, 0] = null;
            arrays[2, 1] = null;
            arrays[2, 2] = null;
            arrays[2, 3] = null;
            arrays[3, 0] = null;
            arrays[3, 1] = null;
            arrays[3, 2] = null;
            arrays[3, 3] = null;
            arrays[3, 4] = null;
            arrays[3, 5] = null;
            arrays[3, 6] = null;
        }
    }
}
