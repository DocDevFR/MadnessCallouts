using System;
using LSPD_First_Response.Mod.API;
using LSPD_First_Response.Mod.Callouts;
using Rage;

namespace MadnessCallouts.Callouts
{
	[CalloutInfo("StolenVehicle", CalloutProbability.High)]
	public class DangerousIndividual : Callout
	{
		private Vector3 SpawnPoint;
		
		private Ped[] suspects;
		
		private bool chaseFired = false;
		
		public override bool OnBeforeCalloutDisplayed()
		{
			SpawnPoint = World.GetNextPositionOnStreet(Game.LocalPlayer.Character.Position.Around(250f));

			ShowCalloutAreaBlipBeforeAccepting(SpawnPoint, 30f);
			AddMinimumDistanceCheck(20f, SpawnPoint);

			CalloutMessage = "Dangerous individual";
			CalloutPosition = SpawnPoint;

			Functions.PlayScannerAudioUsingPosition("WE_HAVE CRIME_SHOTS_FIRED_01 IN_OR_ON_POSITION", SpawnPoint);

			return base.OnBeforeCalloutDisplayed();
		}

		public override bool OnCalloutAccepted()
		{
			initSuspect();
			
			return base.OnCalloutAccepted();
		}

		public override void Process()
		{
			base.Process();
			if (!chaseFired && isDistanceToPeds(suspects, 30f)) {
				chaseFired = true;
			}

			if (isAllSuspectsDown(suspects)) {
				End();
			}
		}

		public override void End()
		{
			base.End();
			dissimAllSuspect(suspects);
		}
		
		private void initSuspect()
		{
			suspects = new Ped[1];
			
			suspects[0] = new Ped(getRandomModel(), SpawnPoint, Game.LocalPlayer.Character.Heading + 100f);

			suspects[0].IsPersistent = true;
			suspects[0].Inventory.GiveNewWeapon("-270015777", -1, true);
		}
		
		private String getRandomModel()
		{
			return "a_m_m_salton_03";
		}
		
		private bool isDistanceToPeds(Ped[] suspectsArray, float distance)
		{
			for (int i = 0; i < suspectsArray.Length; i++) {
				if(Game.LocalPlayer.Character.DistanceTo(suspectsArray[i]) < distance)
				{
					return true;
				}
			}
			
			return false;
		}
		
		private bool isAllSuspectsDown(Ped[] suspectsArray)
		{
			for (int i = 0; i < suspectsArray.Length; i++) {
				Ped currentPed = suspects[i];
				
				if(!currentPed.IsDead || !Functions.IsPedArrested(currentPed) || !Functions.IsPedInPrison(currentPed))
				{
					return false;
				}
			}
			
			return true;
		}
		
		private void dissimAllSuspect(Ped[] suspectsArray)
		{
			for (int i = 0; i < suspectsArray.Length; i++) {
				Ped currentPed = suspects[i];
				if (currentPed.Exists()) {
					currentPed.Dismiss();
				}
			}
		}
		
		private void beAngryToPlayer(Ped[] suspectsArray)
		{
			for (int i = 0; i < suspectsArray.Length; i++) {
				Ped currentPed = suspects[i];
				currentPed.Tasks.FightAgainst(Game.LocalPlayer.Character);
			}
		}
	}
}
