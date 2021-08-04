using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerSkillEpManager : MonoBehaviour
{
    [SerializeField] short karmenQEp;
    [SerializeField] short karmenWEp;
    [SerializeField] short jadeQEp;
    [SerializeField] short jadeWEp;
    [SerializeField] short leinaQEp;
    [SerializeField] short leinaWEp;
    [SerializeField] short evaQEp;
    [SerializeField] short evaWEp;

    public short KarmenQSkill()
    {
        return karmenQEp;
    }

    public short KarmenWSkill()
    {
        return karmenWEp;
    }
    public short JadeQSkill()
    {
        return jadeQEp;
    }

    public short JadeWSkill()
    {
        return jadeWEp;
    }

    public short LeinaQSkill()
    {
        return leinaQEp;
    }
    public short LeinaWSkill()
    {
        return leinaWEp;
    }
    public short EvaQSkill()
    {
        return evaQEp;
    }

    public short EvaWSkill()
    {
        return evaWEp;
    }
}
