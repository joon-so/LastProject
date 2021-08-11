using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSkillEpManager : MonoBehaviour
{
    [SerializeField] short karmenQEp;
    [SerializeField] short karmenWEp;
    [SerializeField] short karmenEEp;
    [SerializeField] short jadeQEp;
    [SerializeField] short jadeWEp;
    [SerializeField] short jadeEEp;
    [SerializeField] short leinaQEp;
    [SerializeField] short leinaWEp;
    [SerializeField] short leinaEEp;
    [SerializeField] short evaQEp;
    [SerializeField] short evaWEp;
    [SerializeField] short evaEEp;

    public short KarmenQSkill()
    {
        return karmenQEp;
    }
    public short KarmenWSkill()
    {
        return karmenWEp;
    }
    public short KarmenESkill()
    {
        return karmenEEp;
    }
    public short JadeQSkill()
    {
        return jadeQEp;
    }
    public short JadeWSkill()
    {
        return jadeWEp;
    }
    public short JadeESkill()
    {
        return jadeEEp;
    }
    public short LeinaQSkill()
    {
        return leinaQEp;
    }
    public short LeinaWSkill()
    {
        return leinaWEp;
    }
    public short LeinaESkill()
    {
        return leinaEEp;
    }
    public short EvaQSkill()
    {
        return evaQEp;
    }
    public short EvaWSkill()
    {
        return evaWEp;
    }
    public short EvaESkill()
    {
        return evaEEp;
    }
}
