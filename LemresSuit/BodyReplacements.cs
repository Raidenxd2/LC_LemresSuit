using ModelReplacement;
using UnityEngine;

namespace ModelReplacement
{
    public class MRLEMRES : BodyReplacementBase
    {
        protected override GameObject LoadAssetsAndReturnModel()
        { 
            string model_name = "lemres";
            return Assets.MainAssetBundle.LoadAsset<GameObject>(model_name);
        }
    }
}