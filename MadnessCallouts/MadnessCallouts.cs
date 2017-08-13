using System;
using LSPD_First_Response.Mod.API;

namespace MadnessCallouts
{
	public class Main : Plugin
	{
		public override void Initialize()
		{
			Functions.OnOnDutyStateChanged += OnOnDutyStateChangedHandler;
			//Game.LogTrivial(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString());
		}
		public override void Finally()
		{
            
		}

		private static void OnOnDutyStateChangedHandler(bool OnDuty)
		{
			if (OnDuty) {
				RegisterCallouts(); 
			}
		}

		private static void RegisterCallouts()
		{
			Functions.RegisterCallout(typeof(Callouts.DangerousIndividual));
		}
	}
}