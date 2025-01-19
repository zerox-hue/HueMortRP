using Life;
using Life.DB;
using Life.Network;
using Life.UI;
using Mirror;
using ModKit.Helper;
using ModKit.Interfaces;
using ModKit.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace HueMortRP
{
    public class HueMortRP : ModKit.ModKit
    {
        public HueMortRP(IGameAPI aPI) : base(aPI)
        {
            PluginInformations = new PluginInformations(AssemblyHelper.GetName(), "1.0.0", "Zerox_Hue");
        }
        public static Config config;
        public class Config
        {
            public int LevelAdminMinForDeleteCharacter;
            public int LevelAdminMinForRecreateCharacter;
        }
        public void CreateConfig()
        {
            string directoryPath = pluginsPath + "/HueMortRP";

            string configFilePath = directoryPath + "/config.json";

            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            if (!File.Exists(configFilePath))
            {
                var defaultConfig = new Config
                {
                    LevelAdminMinForDeleteCharacter = 4,
                    LevelAdminMinForRecreateCharacter = 3,
                };
                string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(defaultConfig, Newtonsoft.Json.Formatting.Indented);
                File.WriteAllText(configFilePath, jsonContent);
            }

            config = Newtonsoft.Json.JsonConvert.DeserializeObject<Config>(File.ReadAllText(configFilePath));
        }
        public override void OnPluginInit()
        {
            base.OnPluginInit();
            CreateConfig();
            ModKit.Internal.Logger.LogSuccess($"{PluginInformations.SourceName} v{PluginInformations.Version}", "initialisé");
            Orm.RegisterTable<HueMortRPOrmCharacterInfo>();
            new SChatCommand("/mortrp", "Mort Rp", "/mortrp", (player, args) => { OnSlashMortRp(player); }).Register();

        }
        public void OnSlashMortRp(Player player)
        {
            UIPanel panel = new UIPanel("Hue Mort RP", UIPanel.PanelType.Tab);
            panel.AddTabLine("<color=#cf171d>Supprimer une personne</color>", ui =>
            {
                if (player.account.AdminLevel >= config.LevelAdminMinForDeleteCharacter)
                {
                    foreach (var players in Nova.server.Players)
                    {
                        UIPanel panel1 = new UIPanel($"<color=#17cf32>Tous les joueurs</color>", UIPanel.PanelType.Tab);
                        panel1.AddTabLine($"<color=#17cf32>{players.GetFullName()}</color>", ui1 =>
                        {
                            UIPanel panel2 = new UIPanel($"<color=red>Supprimer une personne</color>", UIPanel.PanelType.Text);
                            panel2.SetText($"Êtes-vous sûr de vouloir supprimer {players.GetFullName()} ?");
                            panel2.AddButton("Non", ui2 => player.ClosePanel(ui2));
                            panel2.AddButton("Oui", ui2 => { DeleteACharacter(player, players); player.ClosePanel(ui2); });
                            player.ShowPanelUI(panel2);
                        });
                        panel1.AddButton("Fermer", ui1 => player.ClosePanel(ui1));
                        panel1.AddButton("Valider", ui1 => ui1.SelectTab());
                        player.ShowPanelUI(panel1);
                    }
                }
                else
                {
                    player.SendText($"<color=red>[HueMortRP]</color> Vous n'avez pas le niveau d'administration ({config.LevelAdminMinForDeleteCharacter}) requis pour effectuer cette action");
                    return;
                }
            });
            panel.AddTabLine("<color=#175acf>Restaurer le personnage</color>", ui =>
            {
                if (player.account.adminLevel >= config.LevelAdminMinForRecreateCharacter)
                {
                    UIPanel panel1 = new UIPanel($"<color=#17cf32>Nom</color>", UIPanel.PanelType.Input);
                    panel1.AddButton("Fermer", ui1 => player.ClosePanel(ui1));
                    panel1.SetText("Saisissez le nom du personnage à restaurer");
                    panel1.SetInputPlaceholder("Saisissez le nom du personnage à restaurer...");
                    panel1.AddButton("Valider", ui1 =>
                    {
                        UIPanel panel2 = new UIPanel("<color=#17cf32>Prénom</color>", UIPanel.PanelType.Input);
                        panel2.AddButton("Fermer", ui2 => player.ClosePanel(ui2));
                        panel2.SetInputPlaceholder("Saisissez le prénom du personnage à restaurer...");
                        panel2.AddButton("Valider", ui2 =>
                        {
                            char inputText = panel2.inputText[0];
                            string withoutfirstCharacter = panel2.inputText.Substring(1);
                            string Name = inputText.ToString().ToUpper() + withoutfirstCharacter; RestoreAChararcter(Name, panel1.inputText.ToUpper(), player); player.ClosePanel(ui2);
                        });
                        player.ShowPanelUI(panel2);
                    });
                    player.ShowPanelUI(panel1);
                }
                else
                {
                    player.SendText($"<color=red>[HueMortRP]</color> Vous n'avez pas le niveau d'administration ({config.LevelAdminMinForRecreateCharacter}) requis pour effectuer cette action");
                    return;
                }
            });
            panel.AddButton("Fermer", ui => player.ClosePanel(ui));
            panel.AddButton("Valider", ui => ui.SelectTab());
            player.ShowPanelUI(panel);
        }
        public async void DeleteACharacter(Player player, Player target)
        {
            var instance = new HueMortRPOrmCharacterInfo();
            instance.AccountId = target.character.AccountId;
            instance.Bank = target.character.Bank;
            instance.Birthday = target.character.Birthday;
            instance.BizId = target.character.BizId;
            instance.Cash = target.character.Money;
            instance.CharacterId = target.character.Id;
            instance.Commune = target.character.Commune;
            instance.DrugTime = target.character.DrugTime;
            instance.FirstName = target.character.Firstname;
            instance.FullIdCard = target.character.FullIdCard;
            instance.HasBCR = target.character.HasBCR;
            instance.HasCode = target.character.HasCode;
            instance.Health = target.character.Health;
            instance.Height = target.character.Height;
            instance.Hunger = target.character.Hunger;
            instance.IdCard = target.character.IdCard;
            instance.Inventory = target.character.Inventory;
            instance.LastDisconnect = target.character.LastDisconnect;
            instance.LastName = target.character.Lastname;
            instance.LastPosX = target.character.LastPosX;
            instance.LastPosy = target.character.LastPosY;
            instance.LastPosZ = target.character.LastPosZ;
            instance.Level = target.character.Level;
            instance.PermisB = target.character.PermisB;
            instance.PermisPoints = target.character.PermisPoints;
            instance.PhoneNumber = target.character.PhoneNumber;
            instance.PhotoLink = target.character.Photo;
            instance.Prisontime = target.character.PrisonTime;
            instance.RankId = target.character.RankId;
            instance.SexId = target.character.SexId;
            instance.Skin = target.character.Skin;
            instance.StatCoopper = target.character.StatCopper;
            instance.StatDiamond = target.character.StatDiamond;
            instance.StatRock = target.character.StatRock;
            instance.StatTree = target.character.StatTree;
            instance.Thirst = target.character.Thirst;
            instance.TimeStampPermis = target.character.TimestampPermis;
            instance.UsedCloths = target.character.UsedClothes;
            instance.WhiteListForm = target.character.WhitelistForm;
            instance.WhiteListResponse = target.character.WhitelistResponse;
            instance.Worktime = target.character.WorkTime;
            instance.XP = target.character.XP;
            bool result = await instance.Save();
            if (result)
            {
                player.Notify("Succés", "Le personnage a bien été supprimer !", NotificationManager.Type.Success);
                target.SendText("<color=red>[HueMortRP]</color> Un admin a décidé de supprimer ton personnage merci de contacter les staffs si ce n'est pas votre demande !");
                target.setup.TargetShowCenterText("<color=red>Mort RP</color>", "Tu es mort...", 999999f);
                await Task.Delay(5000);
                await LifeDB.DeleteCharacter(target.character.Id);
                target.Disconnect();
                await target.Save();
            }
            else
            {
                player.Notify("Erreur", "Une erreur est survenue lors de la suppression du personnage", NotificationManager.Type.Error);
                return;
            }
        }
        public async void RestoreAChararcter(string name, string surName, Player player)
        {
            var elements = await HueMortRPOrmCharacterInfo.Query(x => x.FirstName == name && x.LastName == surName);
            if (!elements.Any())
            {
                player.Notify("Erreur", "Aucun enregistrement comportant ce nom et prénom", NotificationManager.Type.Error);
                return;
            }
            else
            {
                
                foreach (var allelements in elements)
                {
                    await LifeDB.CreateCharacter(new Character
                    {
                        bank = allelements.Bank,
                        birthday = allelements.Birthday,
                        bizId = allelements.BizId,
                        commune = allelements.Commune,
                        fullIdCard = allelements.FullIdCard,
                        firstname = allelements.FirstName,
                        height = allelements.Height,
                        idCard = allelements.IdCard,
                        id = allelements.Id,
                        sexId = allelements.SexId,
                        skin = CharacterCustomizationSetup.DeserializeFromJson(allelements.Skin),
                        lastname = allelements.LastName,
                        money = allelements.Cash,
                        photo = allelements.PhotoLink,
                    }, allelements.AccountId);
                }
                player.Notify("Succés", "Le personnage a bien été restaurer !", NotificationManager.Type.Success);
            }
        }
        public override async void OnPlayerSpawnCharacter(Player player, NetworkConnection conn, Characters character)
        {
            base.OnPlayerSpawnCharacter(player, conn, character);
            var elements = await HueMortRPOrmCharacterInfo.Query(x => x.FirstName == player.character.Firstname && x.LastName == player.character.Lastname);
            if (elements.Any())
            {
                foreach (var allelementsinfo in elements)
                {
                    player.AddMoney(-player.character.Money, "Restore");
                    player.AddMoney(allelementsinfo.Cash, "Restore");
                    player.AddBankMoney(-player.character.Bank);
                    player.AddBankMoney(allelementsinfo.Bank);
                    player.character.BizId = allelementsinfo.BizId;
                    player.character.Level = allelementsinfo.Level;
                    player.character.XP = allelementsinfo.XP;
                    player.character.PermisB = allelementsinfo.PermisB;
                    player.character.PermisPoints = allelementsinfo.PermisPoints;
                    player.character.TimestampPermis = allelementsinfo.TimeStampPermis;
                    player.character.RankId = allelementsinfo.RankId;
                    player.character.HasCode = allelementsinfo.HasCode;
                    player.character.Save();
                    await player.Save();
                    allelementsinfo.FirstName = "null";
                    allelementsinfo.LastName = "null";
                    await allelementsinfo.Save();
                    player.SendText("<color=red>[HueMortRP]</color> Bon retour parmis nous !");
                }
            }
        }
    }
}
