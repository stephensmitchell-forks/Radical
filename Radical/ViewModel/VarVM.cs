﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Special;
using Grasshopper.GUI;
using Radical.Integration;

namespace Radical
{
    public class VarVM : BaseVM
    {
        public VarVM()
        {
        }

        public VarVM(IVariable dvar)
        {
            DesignVar = dvar;
            this.Name = DesignVar.Parameter.NickName;
            this.Value=DesignVar.CurrentValue;
            this._min = DesignVar.Min;
            this._max = DesignVar.Max;
            this.IsEnabled = true;
        }

        public IVariable DesignVar;

        private string _name;
        public string Name
        {
            get
            { return _name; }
            set
            {
                if (CheckPropertyChanged<string>("Name", ref _name, ref value))
                {
                    DesignVar.Parameter.NickName = Name;
                }
            }

        }

        private double _value;
        public double Value
        {
            get
            { return _value; }
            set
            {
                if (CheckPropertyChanged<double>("Value", ref _value, ref value))
                {
                    DesignVar.UpdateValue(Value);
                }
            }

        }

        private double _min;
        public double Min
        {
            get
            { return _min; }
            set
            {
                double old_min = _min;
                if (CheckPropertyChanged<double>("Min", ref _min, ref value))
                {
                    if(value > this._max){ //Invalid Bounds, display an error
                        System.Windows.MessageBox.Show(String.Format("Incompatible bounds!\n " +
                                                                        "Min:{0} > Max:{1}\n" +
                                                                        "Resetting min to {2}\n", value, this._max, old_min));
                        this._min = old_min;
                    }
                    else
                    {
                        DesignVar.UpdateMin(_min);

                        //Ensure the value of the slider is not outside the new min bound
                        if (_min > DesignVar.CurrentValue)
                        {
                            DesignVar.UpdateValue(_min);
                            //NEED SOME KIND OF REFRESH AFTER THIS UPDATE
                        }
                    }
                }
            }

        }

        private double _max;
        public double Max
        {
            get
            { return _max; }
            set
            {
                double old_max = _max;
                if (CheckPropertyChanged<double>("Max", ref _max, ref value))
                {
                    if (value < this._min){ //Invalid Bounds, display an error
                        System.Windows.MessageBox.Show(String.Format("Incompatible bounds!\n " +
                                                                        "Max:{0} < Min:{1}\n" +
                                                                        "Resetting max to {2}\n", value, this._min, old_max));
                        this._max = old_max; 
                    }
                    else{
                        DesignVar.UpdateMax(_max);

                        //Ensure the value of the slider is not outside the new max bound
                        if (_max < DesignVar.CurrentValue)
                        {
                            DesignVar.UpdateValue(_max);
                            //NEED SOME KIND OF REFRESH AFTER THIS UPDATE
                        }
                    }
                }
            }

        }

        private bool _isenabled;
        public bool IsEnabled
        {
            get
            {
                return _isenabled;
            }
            set
            {
                if (CheckPropertyChanged<bool>("IsEnabled", ref _isenabled, ref value))
                {
                    DesignVar.IsActive = IsEnabled;
                }
            }
        }
    }
}
