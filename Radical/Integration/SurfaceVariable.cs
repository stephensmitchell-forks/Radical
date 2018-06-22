﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;


namespace Radical.Integration
{
    //Represents a single control point degree of freedom on a NURBS Surface
    public class SurfaceVariable : IGeoVariable
    {
        //dir: The direction in which the point can move
        //min, max: The positional bounds of the control point
        //u, v: NURBS coordinates of the control point
        public int u;
        public int v;
        private int dir;

        public SurfaceVariable(double min, double max, int u, int v, int dir, DesignSurface surf)
        {
            this.dir = dir;
            this.Min = min;
            this.Max = max;
            this.u = u;
            this.v = v;
            this.Geometry = surf;
        }

        public int Dir
        {
            get { return this.dir; }
            set { this.dir = value; }
        }

        public double Max { get; set; }
        public double Min { get; set; }

        public double CurrentValue
        {
            get;set;
        }

        public IDesignGeometry Geometry { get; set; }

        // POTENTIAL ARCHITECTURE FLAW
        public IGH_Param Parameter
        {
            get
            {
                return ((DesignSurface)Geometry).Parameter;
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public bool IsActive
        {
            get; set;
        }


        //For changing the private min and max bounds
        public void UpdateMin(double x)
        {
            this.Min = x;
        }

        public void UpdateMax(double x)
        {
            this.Max = x;
        }

        public double Gradient()
        {
            throw new NotImplementedException();
        }

        public void UpdateValue(double x)
        {
            this.CurrentValue = x;
            Geometry.VarUpdate(this);
        }
    }
}
