using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static ChapterMaster.Data.Enums;
using static ChapterMaster.Data.Structs;

namespace ChapterMaster
{
    public class GenerationScript
    {
        private NameLocalisationStructure names;

        public void Initialize(NameLocalisationStructure names)
        {
            this.names = names;
            ChapterInfo info = new ChapterInfo();
            info.companies = new List<CompanyInfo>();

            for (int i = 0; i < 10; i++)
            {
                CompanyInfo company = new CompanyInfo();
                company.CompanyName = "Company " + i;
                company.CompanyNickname = "Nickname " + i;
                company.CompanyID = i;
                company.Squads = new List<SquadInfo>();

                CreateSquads(company);

                info.companies.Add(company);
            }

            SaveData savedData = new SaveData();
            savedData.chapter = info;

            string path = Application.streamingAssetsPath + "/Save.json";

            string json = JsonUtility.ToJson(savedData);
            File.WriteAllText(path, json);
        }

        private void CreateSquads(CompanyInfo company)
        {
            for (int i = 0; i < 10; i++)
            {
                SquadInfo Squad = new SquadInfo();
                Squad.SquadName = "Squad " + i;
                Squad.SquadID = i;

                switch (i)
                {
                    case < 6:
                        Squad.SquadType = SquadType.Tactical;
                        break;
                    case < 8:
                        Squad.SquadType = SquadType.Assault;
                        break;
                    default:
                        Squad.SquadType = SquadType.Devastator;
                        break;
                }

                Squad.Soldiers = new List<SoldierInfo>();

                CreateSoldiers(Squad);

                company.Squads.Add(Squad);
            }
        }

        private void CreateSoldiers(SquadInfo squad)
        {
            for (int i = 0; i < 10; i++)
            {
                SoldierInfo Soldier = new SoldierInfo();

                int fName = Random.Range(0, names.firstNames.Count);
                int sName = Random.Range(0, names.secondNames.Count);
                Soldier.soldierID = i;
                Soldier.designation = ((SoldierDesignation)squad.SquadType).ToString();
                Soldier.primaryWeapon = "Bolter";
                Soldier.secondaryWeapon = "Bolt Pistol";
                Soldier.meleeWeapon = "Combat Knife";
                Soldier.armour = "MK7";

                Soldier.soldierName = names.firstNames[fName] + " " + names.secondNames[sName];

                squad.Soldiers.Add(Soldier);
            }
        }
    }
}
