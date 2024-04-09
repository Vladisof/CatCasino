using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace BE
{

	//class for random selection
	public class BenChoice
	{
		
		private	List<int> 	IDs = new List<int>();
		private	List<int> 	Values = new List<int>();
		public	int			ValueTotal;

		public void Clear() {
			IDs.Clear();
			Values.Clear();
			ValueTotal = 0;
		}

		public void Add(int id, int value) {
			if(value == 0) return;

			IDs.Add (id);
			Values.Add (value);
			ValueTotal += value;
		}

		public int Choice(int choice) {
			//Debug.Log ("Choice value:"+choice.ToString ());

			if((choice < 0) || (ValueTotal <= choice)) return -1;

			for(int i=0 ; i < Values.Count ; ++i) {
				if(choice < Values[i]) 	return IDs[i];
				else  					choice -= Values[i];
			}

			return -1;
		}
	}
	public class BeNumber {
	
		public enum IncType {
			NONE 			= -1,
			VALUE 			= 0,
			VALUEwithMAX 	= 1,
			TIME 			= 2,
		};
		
		private bool  	bInChange 	= false;
		private float 	fAge 		= 0.0f;
		private float 	fInc 		= 1.0f;
		private double 	fTarget 	= 0.0;
		
		private double 	fMin 		= 0.0;
		private double 	fMax 		= 1.0;
		private double 	fCurrent 	= 0.0;
		
		private IncType eType		= IncType.VALUEwithMAX;
		private string  strFormat	= "#,##0";
		private GameObject 		m_EventTarget = null;
		private string 			m_EventFunction;
		private GameObject 		m_EventParameter;

		private TextMeshProUGUI			UIText = null;
		
		public BeNumber(IncType type, string _strFormat, double min, double max, double current) {
			Init(type, _strFormat, min, max, current);
		}
		
		public void Init(IncType type, string _strFormat, double min, double max, double current) {
			eType = type;
			strFormat = _strFormat;
			fMin = min;
			fMax = max;
			fCurrent = current;
			fTarget = current;
			bInChange = false;
		}

		public void 	AddUIText(TextMeshProUGUI ui)		{ UIText = ui; if(UIText != null) UIText.text = ToString(); }

		public void 	TypeSet(IncType type)	{ eType = type; }
		public IncType 	Type()					{ return eType; }
		
		public bool 	InChange()				{ return bInChange; }
		public float 	Ratio()					{ return (float)((fCurrent-fMin)/(fMax-fMin)); }
		public float 	TargetRatio()			{ return (float)((fTarget-fMin)/(fMax-fMin)); }
		public double 	Current()				{ return fCurrent; }
		public double 	Min()					{ return fMin; }
		public double 	Max()					{ return fMax; }
		public void 	MaxSet(double value)	{ fMax = value; }
		public double 	Target()				{ return fTarget; }
		public override string 	ToString()	 { 
			if(eType == IncType.VALUE) 				return fCurrent.ToString (strFormat); 
			else if(eType == IncType.VALUEwithMAX) 	return fCurrent.ToString (strFormat)+" / "+fMax.ToString (strFormat); 
			else if(eType == IncType.TIME) {
				int iCurrent = (int)fCurrent;
				int Day  = iCurrent/86400;	if(Day > 0)  iCurrent -= Day *86400;
				int Hour = iCurrent/3600;	if(Hour > 0) iCurrent -= Hour*3600;
				int Min  = iCurrent/60;		if(Min > 0)  iCurrent -= Min*60;
				int Sec  = iCurrent;
				
				if(Day > 0) 		return Day.ToString()+ "D "+Hour.ToString ()+"H";
				else if(Hour > 0) 	return Hour.ToString()+"H "+Min.ToString ()+"M";
				else if(Min > 0) 	return Min.ToString()+ "M "+Sec.ToString ()+"S";
				else 				return Sec.ToString()+"S";
			}
			else {
				return "";
			}
		}
		
		public void ChangeTo(double target) {
			if(target < fMin) target = fMin;
			if(target > fMax) target = fMax;
			if(!bInChange) {
				bInChange = true;
				fAge = 0.0f;
				fInc = 1.0f;
			}
			fTarget = target;
		}

		public void ChangeDelta(double target) {
			ChangeTo(fTarget+target);
		}

		public void Update() {
			if(!bInChange) return;
			
			fAge += Time.deltaTime * 6.0f;
			fInc += Mathf.Exp(fAge);
			
			if(fTarget > fCurrent) 	{ fCurrent += (double)fInc; if(fCurrent >= fTarget) End(); }
			else  					{ fCurrent -= (double)fInc; if(fCurrent <= fTarget) End(); }

			if(UIText != null)
				UIText.text = ToString();
		}

		private void End() {
			bInChange = false; 
			fCurrent = fTarget;

			if(m_EventTarget != null)
				m_EventTarget.SendMessage(m_EventFunction, m_EventParameter);
		}

		public void SetReceiver(GameObject target, string functionName, GameObject parameter) {
			m_EventTarget = target;
			m_EventFunction = functionName;
			m_EventParameter = parameter;
		}
	}

}
