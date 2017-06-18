using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

static class InfoText {
    public static animalDescription[] animalDescript;

    public static animalDescription[] getDescriptions() {
        animalDescript = new animalDescription[]
        {
            new animalDescription { name = "Addax", gender = "InfoGenderMale", conservation = "",  map = "T_Map_BohorReedbuck", maxCount = 1},
            new animalDescription { name = "Addax_F", gender = "InfoGenderFemale", conservation = "",  map = "T_Map_BohorReedbuck", maxCount = 2},
            new animalDescription { name = "AdersDuiker", displayName = "Aders Duiker", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 3},
            new animalDescription { name = "Blesbok", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 4},
            new animalDescription { name = "Blesbok_F", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "BlueWildebeest", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "BlueWildebeest_F", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "BohorReedbuck", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "BohorReedbuck_F", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "Bongo", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "Bongo_F", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "Cokes_hartebeest", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "Common_Eland", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "CuviersGazelle", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "CuviersGazelle_F", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "DamaGazelle", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "DamaGazelle_F", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "DorcasGazelle", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "DorcasGazelle_F", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "GiantEland", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "GiantEland_F", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "GreaterKudu", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "GreaterKudu_F", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "HuntersAntelope", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "Impala", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "Impala_F", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "Klipspringer", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "Klipspringer_F", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "Lechwe", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "Lioness", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "Nyala", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "Oribi", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "Oribi_F", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "Puku", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "Puku_F", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "RedFrontedGazelle", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "RedHartebeest", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "Roan", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "Roan_F", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "Sable", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "Suni", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "Suni_F", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "ThornsonsGazelle", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "Topi", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "Waterbuck", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
            new animalDescription { name = "ZebraDuuiker", gender = "InfoGenderMale", conservation = "",  map = "", maxCount = 1},
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
            return m_displayName;
        }
        set
        {
            m_displayName = value;
        }
    }

    private string m_gender = "";
    public string gender
    {
        get
        {
            return m_gender;
        }
        set
        {
            m_gender = value;
        }
    }

    private string m_conservation = "";
    public string conservation
    {
        get
        {
            return m_conservation;
        }
        set
        {
            m_conservation = value;
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
