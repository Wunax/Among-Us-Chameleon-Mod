using PlayerDataInstance = EGLJNOMOGNP.DCJMABDDJCF;

namespace ChameleonMod.API
{
    public class PlayerData
    {
        public PlayerController PlayerController { get; }
        public PlayerDataInstance PlayerDataObject
        {
            get
            {
                return PlayerController.PlayerControl.NDGFFHMFGIG;
            }
        }

        public bool IsImpostor
        {
            get
            {
                return PlayerDataObject.DAPKNDBLKIA;
            }
            set
            {
                PlayerDataObject.DAPKNDBLKIA = value;
            }
        }
        public bool IsDead
        {
            get
            {
                return PlayerDataObject.DLPCKPBIJOE;
            }
            set
            {
                PlayerDataObject.DLPCKPBIJOE = value;
            }
        }

        public PlayerData(PlayerController controller)
        {
            PlayerController = controller;
        }

        public PlayerData(PlayerDataInstance data)
        {
            PlayerController = new PlayerController(data.LAOEJKHLKAI);
        }
    }
}