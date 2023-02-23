using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static ChapterMaster.Data.Enums;
using static ChapterMaster.Data.Structs;

namespace ChapterMaster
{
    public class GenerationTestScript : MonoBehaviour
    {
        public OrganisationPage page;
        private NameLocalisationStructure names;

        public bool GenerateNames = true;

        void Start()
        {
            string path = Application.streamingAssetsPath + "/Configs/SoldierNames.json";
            string newPath = Application.streamingAssetsPath + "/Configs/WeaponData.json";

            if (GenerateNames == true)
            {
                WeaponData weapons = new WeaponData();
                weapons.weapons = new List<Weapon>();

                Weapon weapon = new Weapon();
                weapon.weaponName = "Bolter";
                weapon.accuracy = 75f;
                weapon.fireRate = 0.5f;
                weapon.damage = 10f;
                weapon.model = "BolterStandard";
                weapon.sound = "SingleShotBolter";

                weapons.weapons.Add(weapon);

                weapon.weaponName = "Bolt Pistol";
                weapon.accuracy = 55f;
                weapon.fireRate = 1f;
                weapon.damage = 8f;
                weapon.model = "BoltPistolStandard";
                weapon.sound = "SingleShotBolter";

                weapons.weapons.Add(weapon);

                string json = JsonUtility.ToJson(weapons);
                File.WriteAllText(newPath, json);
            }

            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                names = JsonUtility.FromJson<NameLocalisationStructure>(json);
            }

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

            ChapterModel model = new ChapterModel();
            model.Load(info);

            page.Load(model, info);
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
                Soldier.primaryWeapon = "Bolter";
                Soldier.secondaryWeapon = "Bolt Pistol";
                Soldier.meleeWeapon = "Knife";
                Soldier.armour = "MK 7";

                Soldier.firstName = names.firstNames[fName];
                Soldier.secondName = names.secondNames[sName];

                squad.Soldiers.Add(Soldier);
            }
        }
    }
}
