using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EDDiscovery.Forms
{
    public partial class ShipDetails : Form
    {
        public class FSDSpec
        {
            private int fsdClass;
            private string rating;
            private double pc;
            private double lc;
            private double mOpt;
            private double mfpj;

            public int FsdClass { get { return fsdClass; } }
            public string Rating { get { return rating; } }
            public double PowerConstant { get { return pc; } }
            public double LinearConstant { get { return lc; } }
            public double OptimalMass { get { return mOpt; } }
            public double MaxFuelPerJump { get { return mfpj; } }

            public FSDSpec(int fsdClass,
                string rating,
                double pc,
                double lc,
                double mOpt,
                double mfpj)
            {

                this.fsdClass = fsdClass;
                this.rating = rating;
                this.pc = pc;
                this.lc = lc;
                this.lc = lc;
                this.mOpt = mOpt;
                this.mfpj = mfpj;
            }



        }


        public static readonly ReadOnlyCollection<FSDSpec> fsdList = new ReadOnlyCollection<FSDSpec>(
    new FSDSpec[] { new FSDSpec(2, "E", 2, 11, 48, 0.6),
                        new FSDSpec(2,  "D" ,   2   ,10 ,54 ,0.6),
        new FSDSpec(2,  "C" ,   2   ,8  ,60 ,0.6),
        new FSDSpec(2,  "B" ,   2   ,10 ,75,    0.8),
        new FSDSpec(2   ,"A",   2,  12, 90, 0.9),
        new FSDSpec(3,  "E",    2.15    ,11 ,80,    1.2),
        new FSDSpec(3,  "D",    2.15    ,10 ,90,    1.2),
        new FSDSpec(3,  "C" ,   2.15,   8   ,100,   1.2),
        new FSDSpec(3,  "B",    2.15,   10  ,125    ,1.5),
        new FSDSpec(3,  "A" ,   2.15    ,12 ,150    ,1.8),
        new FSDSpec(4,  "E",    2.3,    11  ,280,   2),
        new FSDSpec(4,  "D" ,   2.3,    10, 315 ,2),
        new FSDSpec(4,  "C"     ,2.3    ,8  ,350,   2),
        new FSDSpec(4,  "B" ,   2.3 ,10,    438,    2.5),
        new FSDSpec(4   ,"A",   2.3,    12  ,525    ,3),
        new FSDSpec(5,  "E" ,   2.45,   11, 560 ,3.3),
        new FSDSpec(5,  "D" ,   2.45    ,10 ,630,   3.3),
        new FSDSpec(5   ,"C"    ,2.45,  8   ,700    ,3.3),
        new FSDSpec(5   ,"B"    ,2.45   ,10 ,875    ,4.1),
        new FSDSpec(5   ,"A"    ,2.45   ,12 ,1050   ,5),
        new FSDSpec(6   ,"E"    ,2.6    ,11 ,960    ,5.3),
        new FSDSpec(6   ,"D"    ,2.6    ,10 ,1080   ,5.3),
        new FSDSpec(6   ,"C"    ,2.6    ,8  ,1200   ,5.3),
        new FSDSpec(6   ,"B"    ,2.6    ,10 ,1500   ,6.6),
        new FSDSpec(6   ,"A"    ,2.6    ,12 ,1800   ,8),
        new FSDSpec(7,  "E"     ,2.75   ,11 ,1440   ,8.5),
        new FSDSpec(7   ,"D"    ,2.75   ,10 ,1620   ,8.5),
        new FSDSpec(7   ,"C"    ,2.75   ,8, 1800    ,8.5),
        new FSDSpec(7   ,"B"    ,2.75   ,10 ,2250   ,10.6),
        new FSDSpec(7   ,"A"    ,2.75   ,12 ,2700   ,12.8)
    });



        public string FSDDrive { get { return fsdDrive.Text; } set { fsdDrive.SelectedIndex = fsdDrive.FindStringExact(value); } }
        public double LinearConstant
        {
            get
            {
                double value = -1;
                return (Double.TryParse(fsdLinearConstant.Text, out value) ? value : -1);
            }
            set { fsdLinearConstant.Text = value.ToString("0.00"); }
        }
        public double UnladenMass
        {
            get
            {
                double value = -1;
                return (Double.TryParse(unladenMass.Text, out value) ? value : -1);
            }
            set { unladenMass.Text = "" + value.ToString("0.00"); }
        }
        public double OptimalMass
        {
            get
            {
                double value = -1;
                return (Double.TryParse(fsdOptimalMass.Text, out value) ? value : -1);
            }
            set { fsdOptimalMass.Text = "" + value.ToString("0.00"); }
        }
        public double PowerConstant
        {
            get
            {
                double value = -1;
                return (Double.TryParse(fsdPowerConstant.Text, out value) ? value : -1);
            }
            set { fsdPowerConstant.Text = "" + value.ToString("0.00"); }
        }

        public double maxFuelPerJump
        {
            get
            {
                double value = -1;
                return (Double.TryParse(fsdMaxFuelPerJump.Text, out value) ? value : -1);
            }
            set { fsdMaxFuelPerJump.Text = "" + value.ToString("0.00"); }
        }
        public double CurrentCargo
        {
            get
            {
                double value = -1;
                return (Double.TryParse(currentCargo.Text, out value) ? value : -1);
            }
            set { currentCargo.Text = "" + value.ToString("0.00"); }
        }

        public double tankWarning
        {
            get
            {
                double value = -1;
                return (Double.TryParse(TankWarning.Text, out value) ? value : -1);
            }
            set { TankWarning.Text = "" + value.ToString("0.00"); }
        }

        public double TankSize
        {
            get
            {
                double value = -1;
                return (Double.TryParse(tankSize.Text, out value) ? value : -1);
            }
            set { tankSize.Text = "" + value.ToString("0.00"); }
        }
        public double JumpRange
        {
            get
            {
                double value = -1;
                return (Double.TryParse(fsdJumpRange.Text, out value) ? value : -1);
            }
            set { fsdJumpRange.Text = "" + value.ToString("0.00"); }
        }


        public ShipDetails()
        {
            InitializeComponent();
            fsdDrive.DisplayMember = "Key";
            fsdDrive.ValueMember = "Value";
            foreach (FSDSpec spec in fsdList)
            {
                fsdDrive.Items.Add(new KeyValuePair<String, FSDSpec>(String.Format("{0}{1}", spec.FsdClass, spec.Rating), spec));
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void btnUpdateFSD_Click(object sender, EventArgs e)
        {
            //  string fsdnname = fsdDrive.SelectedValue;
            if (fsdDrive.SelectedIndex == -1)
                return;
            KeyValuePair<String, FSDSpec> keyp = (KeyValuePair<String, FSDSpec>)fsdDrive.SelectedItem;


            FSDSpec spec = keyp.Value;
            fsdMaxFuelPerJump.Text = "" + spec.MaxFuelPerJump;
            fsdOptimalMass.Text = "" + spec.OptimalMass;
            fsdPowerConstant.Text = "" + spec.PowerConstant;
            fsdLinearConstant.Text = "" + spec.LinearConstant;
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {
            if (maxFuelPerJump > 0
                && LinearConstant > 0
                && PowerConstant > 0
                && OptimalMass > 0
                && CurrentCargo >= 0
                && UnladenMass > 0
                && TankSize > 0
                )
            {
                JumpRange = Math.Pow(maxFuelPerJump / (LinearConstant * 0.001), 1 / PowerConstant) * OptimalMass / (CurrentCargo + UnladenMass + TankSize);
            }
        }
    }
}
