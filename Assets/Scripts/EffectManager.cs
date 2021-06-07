using UnityEngine;

public class EffectManager : MonoBehaviour
{
  private static EffectManager m_Instance;
  public static EffectManager Instance
  {
      get{
          if(m_Instance == null) m_Instance = FindObjectOfType<EffectManager>();
          return m_Instance;
      }
  }

  public enum EffectType
  {
      Common,
      Flesh
  }

  public ParticleSystem CommonHitEffectPrefab;
  public ParticleSystem fleshHitEffectPrefab;

  public void PlayHitEffect(Vector3 pos, Vector3 normal, Transform parent = null, EffectType effectType = EffectType.Common)
  {
      var targetPrefab = CommonHitEffectPrefab;
      if(effectType == EffectType.Flesh)
      {
          targetPrefab = fleshHitEffectPrefab;
      }

      //var effect = insta
  }

}
