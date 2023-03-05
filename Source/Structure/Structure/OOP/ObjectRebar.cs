using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.DB;

namespace Structure.OOP
{
    class ObjectRebar
    {
        public Rebar CreateFromCurve(string _rebarStyle,RebarBarType _rebarTyle, RebarHookType _startHookTyle, RebarHookType _endHookTyle, Element _host, XYZ _vector,Curve _curve,string _startHookOrientation,string _endHookOrientation)
        {
			Enum.TryParse<RebarStyle>(_rebarStyle, out RebarStyle style);
			var rebarType = _rebarTyle;
			var startHook = _startHookTyle;
			var endHook = _endHookTyle;
			var host = _host;
			var norm = _vector;
			var curves = new List<Curve> { (Curve)_curve };
			Enum.TryParse<RebarHookOrientation>(_startHookOrientation, out RebarHookOrientation startHookOrient);
			Enum.TryParse<RebarHookOrientation>(_endHookOrientation, out RebarHookOrientation endHookOrient);
			var useExistingShapeIfPossible = true;
			var createNewShape = true;
			Rebar rebar = Autodesk.Revit.DB.Structure.Rebar.CreateFromCurves(VarGlobal.doc, style, rebarType, startHook, endHook, host, norm, curves, startHookOrient, endHookOrient, useExistingShapeIfPossible, createNewShape);
			return rebar;
		}
		public Rebar CreateFromCurves(string _rebarStyle, RebarBarType _rebarTyle, RebarHookType _startHookTyle, RebarHookType _endHookTyle, Element _host, XYZ _vector, List<Curve> _curves, string _startHookOrientation, string _endHookOrientation)
		{
			Enum.TryParse<RebarStyle>(_rebarStyle, out RebarStyle style);
			var rebarType = _rebarTyle;
			var startHook = _startHookTyle;
			var endHook = _endHookTyle;
			var host = _host;
			var norm = _vector;
			var curves = _curves;
			Enum.TryParse<RebarHookOrientation>(_startHookOrientation, out RebarHookOrientation startHookOrient);
			Enum.TryParse<RebarHookOrientation>(_endHookOrientation, out RebarHookOrientation endHookOrient);
			var useExistingShapeIfPossible = true;
			var createNewShape = true;
			Rebar rebar = Autodesk.Revit.DB.Structure.Rebar.CreateFromCurves(VarGlobal.doc, style, rebarType, startHook, endHook, host, norm, curves, startHookOrient, endHookOrient, useExistingShapeIfPossible, createNewShape);
			return rebar;
		}
		public Rebar SetLayoutAsMaximumSpacing(Rebar _rebar,double _spacing,double _arrayLength,bool _barsOnNormalSide,bool _includeFirstBar,bool _includeLastBar)
        {
			_rebar.GetShapeDrivenAccessor().SetLayoutAsMaximumSpacing(_spacing / 304.8, _arrayLength / 304.8, _barsOnNormalSide, _includeFirstBar, _includeLastBar);
			return _rebar;

		}
	}
}
