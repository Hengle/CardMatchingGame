using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

static class InfoText {
    public static animalDescription[] animalDescript;

    public static animalDescription[] getDescriptions() {
        animalDescript = new animalDescription[]
        {
			new animalDescription { name = "Addax", displayName = "Addax", gender = "InfoGenderMale", conservation = "Critically Endangered",  map = "T_Map_BohorReedbuck", maxCount = 3},
			new animalDescription { name = "Addax_F", displayName = "Addax", gender = "InfoGenderFemale", conservation = "Critically Endangered",  map = "T_Map_BohorReedbuck", maxCount = 3},
			new animalDescription { name = "AdersDuiker", displayName = "Ader's Duiker", gender = "InfoGenderMale", conservation = "Critically Endangered",  map = "T_Map_BohorReedbuck", maxCount = 3},
			new animalDescription { name = "Blesbok", displayName = "Blesbok", gender = "InfoGenderMale", conservation = "Least Concern",  map = "T_Map_BohorReedbuck", maxCount = 6},
			new animalDescription { name = "Blesbok_F", displayName = "Blesbok", gender = "InfoGenderFemale", conservation = "Least Concern",  map = "T_Map_BohorReedbuck", maxCount = 6},
			new animalDescription { name = "BlueWildebeest", displayName = "Blue Wildebeest", gender = "InfoGenderMale", conservation = "Least Concern",  map = "T_Map_BohorReedbuck", maxCount = 21},
			new animalDescription { name = "BlueWildebeest_F", displayName = "Blue Wildebeest", gender = "InfoGenderFemale", conservation = "Least Concern",  map = "T_Map_BohorReedbuck", maxCount = 21},
			new animalDescription { name = "BohorReedbuck", displayName = "Bohor Reedbuck", gender = "InfoGenderMale", conservation = "Least Concern",  map = "T_Map_BohorReedbuck", maxCount = 36},
			new animalDescription { name = "BohorReedbuck_F", displayName = "Bohor Reedbuck", gender = "InfoGenderFemale", conservation = "Least Concern",  map = "T_Map_BohorReedbuck", maxCount = 36},
			new animalDescription { name = "Bongo", displayName = "Bongo", gender = "InfoGenderMale", conservation = "Near Threatened",  map = "T_Map_BohorReedbuck", maxCount = 9},
			new animalDescription { name = "Bongo_F", displayName = "Bongo", gender = "InfoGenderFemale", conservation = "Near Threatened",  map = "T_Map_BohorReedbuck", maxCount = 9},
			new animalDescription { name = "Cokes_hartebeest", displayName = "Slender-horned Gazelle", gender = "InfoGenderMale", conservation = "",  map = "T_Map_BohorReedbuck", maxCount = 1},
			new animalDescription { name = "Common_Eland", displayName = "Giant Eland", gender = "InfoGenderMale", conservation = "Least Concern",  map = "T_Map_BohorReedbuck", maxCount = 1},
			new animalDescription { name = "CuviersGazelle", displayName = "Cuviers Gazelle", gender = "InfoGenderMale", conservation = "Vulnerable",  map = "T_Map_BohorReedbuck", maxCount = 6},
			new animalDescription { name = "CuviersGazelle_F", displayName = "Cuviers Gazelle", gender = "InfoGenderFemale", conservation = "Vulnerable",  map = "T_Map_BohorReedbuck", maxCount = 6},
			new animalDescription { name = "DamaGazelle", displayName = "Dama Gazelle", gender = "InfoGenderMale", conservation = "Critically Endangered",  map = "T_Map_BohorReedbuck", maxCount = 9},
			new animalDescription { name = "DamaGazelle_F", displayName = "Dama Gazelle", gender = "InfoGenderFemale", conservation = "Critically Endangered",  map = "T_Map_BohorReedbuck", maxCount = 9},
			new animalDescription { name = "DorcasGazelle", displayName = "Dorcas Gazelle", gender = "InfoGenderMale", conservation = "Vulnerable",  map = "T_Map_BohorReedbuck", maxCount = 9},
			new animalDescription { name = "DorcasGazelle_F", displayName = "Dorcas Gazelle", gender = "InfoGenderFemale", conservation = "Vulnerable",  map = "T_Map_BohorReedbuck", maxCount = 9},
			new animalDescription { name = "GiantEland", displayName = "Giant Eland", gender = "InfoGenderMale", conservation = "Least Concern",  map = "T_Map_BohorReedbuck", maxCount = 12},
			new animalDescription { name = "GiantEland_F", displayName = "Giant Eland", gender = "InfoGenderFemale", conservation = "Least Concern",  map = "T_Map_BohorReedbuck", maxCount = 12},
			new animalDescription { name = "GreaterKudu", displayName = "Greater Kudu", gender = "InfoGenderMale", conservation = "Least Concern",  map = "T_Map_BohorReedbuck", maxCount = 27},
			new animalDescription { name = "GreaterKudu_F", displayName = "Greater Kudu", gender = "InfoGenderFemale", conservation = "Least Concern",  map = "T_Map_BohorReedbuck", maxCount = 27},
			new animalDescription { name = "HuntersAntelope", displayName = "Hunter's Antelope", gender = "InfoGenderMale", conservation = "Critically Endangered",  map = "T_Map_BohorReedbuck", maxCount = 3},
			new animalDescription { name = "Impala", displayName = "Impala", gender = "InfoGenderMale", conservation = "Least Concern",  map = "T_Map_BohorReedbuck", maxCount = 27},
			new animalDescription { name = "Impala_F", displayName = "Impala", gender = "InfoGenderFemale", conservation = "Least Concern",  map = "T_Map_BohorReedbuck", maxCount = 27},
			new animalDescription { name = "Klipspringer", displayName = "Klipspringer", gender = "InfoGenderMale", conservation = "Least Concern",  map = "T_Map_BohorReedbuck", maxCount = 30},
			new animalDescription { name = "Klipspringer_F", displayName = "Klipspringer", gender = "InfoGenderFemale", conservation = "Least Concern",  map = "T_Map_BohorReedbuck", maxCount = 30},
			new animalDescription { name = "Lechwe", displayName = "Nile Lechwe", gender = "InfoGenderMale", conservation = "Endangered",  map = "T_Map_BohorReedbuck", maxCount = 3},
			new animalDescription { name = "Lioness", displayName = "Lioness", gender = "InfoGenderFemale", conservation = "",  map = "T_Map_BohorReedbuck", maxCount = 1},
			new animalDescription { name = "Nyala", displayName = "Mountain Nyala", gender = "InfoGenderMale", conservation = "Endangered",  map = "T_Map_BohorReedbuck", maxCount = 3},
			new animalDescription { name = "Oribi", displayName = "Oribi", gender = "InfoGenderMale", conservation = "Least Concern",  map = "T_Map_BohorReedbuck", maxCount = 45},
			new animalDescription { name = "Oribi_F", displayName = "Oribi", gender = "InfoGenderFemale", conservation = "Least Concern",  map = "T_Map_BohorReedbuck", maxCount = 45},
			new animalDescription { name = "Puku", displayName = "Puku", gender = "InfoGenderMale", conservation = "Near Threatened",  map = "T_Map_BohorReedbuck", maxCount = 9},
			new animalDescription { name = "Puku_F", displayName = "Puku", gender = "InfoGenderFemale", conservation = "Near Threatened",  map = "T_Map_BohorReedbuck", maxCount = 9},
			new animalDescription { name = "RedFrontedGazelle", displayName = "Red-fronted Gazelle", gender = "InfoGenderMale", conservation = "Vulnerable",  map = "T_Map_BohorReedbuck", maxCount = 9},
			new animalDescription { name = "RedHartebeest", displayName = "Red Hartebeest", gender = "InfoGenderMale", conservation = "Least Concern",  map = "T_Map_BohorReedbuck", maxCount = 42},
			new animalDescription { name = "Roan", displayName = "Roan Antelope", gender = "InfoGenderMale", conservation = "Least Concern",  map = "T_Map_BohorReedbuck", maxCount = 36},
			new animalDescription { name = "Roan_F", displayName = "Roan Antelope", gender = "InfoGenderFemale", conservation = "Least Concern",  map = "T_Map_BohorReedbuck", maxCount = 36},
			new animalDescription { name = "Sable", displayName = "Sable Antelope", gender = "InfoGenderMale", conservation = "Least Concern",  map = "T_Map_BohorReedbuck", maxCount = 18},
			new animalDescription { name = "Suni", displayName = "Suni", gender = "InfoGenderMale", conservation = "Least Concern",  map = "T_Map_BohorReedbuck", maxCount = 15},
			new animalDescription { name = "Suni_F", displayName = "Suni", gender = "InfoGenderFemale", conservation = "Least Concern",  map = "T_Map_BohorReedbuck", maxCount = 15},
			new animalDescription { name = "ThomsonsGazelle", displayName = "Thomsons Gazelle", gender = "InfoGenderMale", conservation = "Near Threatened",  map = "T_Map_BohorReedbuck", maxCount = 9},
			new animalDescription { name = "Topi", displayName = "Topi", gender = "InfoGenderMale", conservation = "Least Concern",  map = "T_Map_BohorReedbuck", maxCount = 21},
			new animalDescription { name = "Waterbuck", displayName = "Waterbuck", gender = "InfoGenderMale", conservation = "Least Concern",  map = "T_Map_BohorReedbuck", maxCount = 51},
			new animalDescription { name = "ZebraDuiker", displayName = "Zebra Duiker", gender = "InfoGenderMale", conservation = "Vulnerable",  map = "T_Map_BohorReedbuck", maxCount = 3},
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
