using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

static class InfoText {
    public static animalDescription[] animalDescript;

    public static animalDescription[] getDescriptions() {
        animalDescript = new animalDescription[]
        {
            new animalDescription { name = "Addax", description = "", map = "T_Map_BohorReedbuck", maxCount = 1},
            new animalDescription { name = "Addax_F", description = "", map = "", maxCount = 2},
            new animalDescription { name = "AdersDuiker", description = "", map = "", maxCount = 3},
            new animalDescription { name = "Blesbok", description = "", map = "", maxCount = 4},
            new animalDescription { name = "Blesbok_F", description = "", map = "", maxCount = 1},
            new animalDescription { name = "BlueWildebeest", description = "", map = "", maxCount = 1},
            new animalDescription { name = "BlueWildebeest_F", description = "", map = "", maxCount = 1},
            new animalDescription { name = "BohorReedbuck", description = "", map = "", maxCount = 1},
            new animalDescription { name = "BohorReedbuck_F", description = "", map = "", maxCount = 1},
            new animalDescription { name = "Bongo", description = "", map = "", maxCount = 1},
            new animalDescription { name = "Bongo_F", description = "", map = "", maxCount = 1},
            new animalDescription { name = "Cokes_hartebeest", description = "", map = "", maxCount = 1},
            new animalDescription { name = "Common_Eland", description = "", map = "", maxCount = 1},
            new animalDescription { name = "CuviersGazelle", description = "", map = "", maxCount = 1},
            new animalDescription { name = "CuviersGazelle_F", description = "", map = "", maxCount = 1},
            new animalDescription { name = "DamaGazelle", description = "", map = "", maxCount = 1},
            new animalDescription { name = "DamaGazelle_F", description = "", map = "", maxCount = 1},
            new animalDescription { name = "DorcasGazelle", description = "", map = "", maxCount = 1},
            new animalDescription { name = "DorcasGazelle_F", description = "", map = "", maxCount = 1},
            new animalDescription { name = "GiantEland", description = "", map = "", maxCount = 1},
            new animalDescription { name = "GiantEland_F", description = "", map = "", maxCount = 1},
            new animalDescription { name = "GreaterKudu", description = "", map = "", maxCount = 1},
            new animalDescription { name = "GreaterKudu_F", description = "", map = "", maxCount = 1},
            new animalDescription { name = "HuntersAntelope", description = "", map = "", maxCount = 1},
            new animalDescription { name = "Impala", description = "", map = "", maxCount = 1},
            new animalDescription { name = "Impala_F", description = "", map = "", maxCount = 1},
            new animalDescription { name = "Klipspringer", description = "", map = "", maxCount = 1},
            new animalDescription { name = "Klipspringer_F", description = "", map = "", maxCount = 1},
            new animalDescription { name = "Lechwe", description = "", map = "", maxCount = 1},
            new animalDescription { name = "Lioness", description = "", map = "", maxCount = 1},
            new animalDescription { name = "Nyala", description = "", map = "", maxCount = 1},
            new animalDescription { name = "Oribi", description = "", map = "", maxCount = 1},
            new animalDescription { name = "Oribi_F", description = "", map = "", maxCount = 1},
            new animalDescription { name = "Puku", description = "", map = "", maxCount = 1},
            new animalDescription { name = "Puku_F", description = "", map = "", maxCount = 1},
            new animalDescription { name = "RedFrontedGazelle", description = "", map = "", maxCount = 1},
            new animalDescription { name = "RedHartebeest", description = "", map = "", maxCount = 1},
            new animalDescription { name = "Roan", description = "", map = "", maxCount = 1},
            new animalDescription { name = "Roan_F", description = "", map = "", maxCount = 1},
            new animalDescription { name = "Sable", description = "", map = "", maxCount = 1},
            new animalDescription { name = "Suni", description = "", map = "", maxCount = 1},
            new animalDescription { name = "Suni_F", description = "", map = "", maxCount = 1},
            new animalDescription { name = "ThornsonsGazelle", description = "", map = "", maxCount = 1},
            new animalDescription { name = "Topi", description = "", map = "", maxCount = 1},
            new animalDescription { name = "Waterbuck", description = "", map = "", maxCount = 1},
            new animalDescription { name = "ZebraDuuiker", description = "", map = "", maxCount = 1},
        };

        return animalDescript;
    }
}
public class animalDescription
{
    private string m_name = "";
    public string name
    {
        get
        {
            return m_name;
        }
        set
        {
            m_name = value;
        }
    }

    private string m_displayName = "";
    public string displayName
    {
        get
        {
            return m_name;
        }
        set
        {
            m_name = value;
        }
    }

    private string m_description = "";
    public string description
    {
        get
        {
            return m_description;
        }
        set
        {
            m_description = value;
        }
    }

    private string m_map = "";
    public string map
    {
        get
        {
            return m_map;
        }
        set
        {
            m_map = value;
        }
    }

    private float m_maxCount = 0;
    public float maxCount
    {
        get
        {
            return m_maxCount;
        }
        set
        {
            m_maxCount = value;
        }
    }
}
