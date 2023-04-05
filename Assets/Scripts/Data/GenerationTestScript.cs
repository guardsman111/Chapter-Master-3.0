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
            string newPath = Application.streamingAssetsPath + "/Configs/EquipmentData.json";
            string savePath = Application.streamingAssetsPath + "/Save.json";

            if (GenerateNames == true)
            {
                EquipmentData weapons = new EquipmentData();
                weapons.weapons = new List<Weapon>();

                Weapon weapon = new Weapon();
                weapon.weaponName = "Bolter";
                weapon.type = "Primary";
                weapon.accuracy = 75f;
                weapon.range = 50f;
                weapon.fireRate = 0.5f;
                weapon.magCapacity = 30f;
                weapon.damage = 10f;
                weapon.piercing = 4.0f;
                weapon.model = "BolterStandard";
                weapon.sound = "SingleShotBolter";

                weapons.weapons.Add(weapon);

                weapon.weaponName = "Bolt Pistol";
                weapon.type = "Secondary";
                weapon.accuracy = 55f;
                weapon.range = 40f;
                weapon.fireRate = 1f;
                weapon.magCapacity = 10f;
                weapon.damage = 8f;
                weapon.piercing = 4.0f;
                weapon.model = "BoltPistolStandard";
                weapon.sound = "SingleShotBolter";

                weapons.weapons.Add(weapon);

                weapon.weaponName = "Combat Knife";
                weapon.type = "Melee";
                weapon.accuracy = 85f;
                weapon.range = 0f;
                weapon.fireRate = 1f;
                weapon.magCapacity = 0f;
                weapon.damage = 5f;
                weapon.piercing = 2.0f;
                weapon.model = "Knife";
                weapon.sound = "KnifeStab";

                weapons.weapons.Add(weapon);

                Armour armour = new Armour();
                armour.armourName = "MK7";
                armour.type = "Power";
                armour.protectionLevel = 7.0f;
                armour.coverage = 90f;
                armour.model = "PowerArmourMark7";
                armour.footStepSound = "PowerArmourStep";

                weapons.armours = new List<Armour>();

                weapons.armours.Add(armour);

                string json = JsonUtility.ToJson(weapons);
                File.WriteAllText(newPath, json);
            }


            ChapterModel model = new ChapterModel();
            ChapterInfo info = new ChapterInfo();

            if (File.Exists(savePath))
            {
                string data = File.ReadAllText(savePath);
                info = JsonUtility.FromJson<ChapterInfo>(data);
                model.Load(info);

                page.Load(model, info);
                return;
            }

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
