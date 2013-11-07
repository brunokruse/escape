using UnityEngine;
using System.Collections;

public class PlayerStats : MonoBehaviour {
	
	
	public int PPrimaryWeapon;
	public int PSecondaryWeapon;
	public int PHealth;
	public int PShield;
	public int PThrusters;
	public int PTurnRate;
	
	
	// primary weapon
	public int W1Damage;
	public int W1FireRate;
	public int W1Range;
	
	// secondary weapon
	public int W2Damage;
	public int W2FireRate;
	public int W2Range;
	
	
	public int totalPoints = 14;
	
	// Use this for initialization
	void Start () {
		resetStats();
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	// player stats
	public void addHealth () {
		int tmpPHealth = PlayerPrefs.GetInt("PHealth");
		tmpPHealth += 1;
		
		PlayerPrefs.SetInt("PHealth", tmpPHealth);
		
		totalPoints -= 1;
	}
	
	public void addShield () {
		int tmpPShield = PlayerPrefs.GetInt("PShield");
		PShield += 1;

		PlayerPrefs.SetInt("PShield", PShield);
		
		totalPoints -= 1;
	}
	
	public void addThrusters () {
		int tmpPThrusters = PlayerPrefs.GetInt("PThrusters");
		PThrusters += 1;
		
		PlayerPrefs.SetInt("PThrusters", PThrusters);
		
		totalPoints -= 1;
	}
	
	public void addTurnRate () {
		int tmpPTurnRate = PlayerPrefs.GetInt("PTurnRate");
		PTurnRate += 1;

		PlayerPrefs.SetInt("PTurnRate", PTurnRate);

		totalPoints -= 1;
	}
	
	// weapon 1
	public void addW1Damage () {
		int tmpW1Damage = PlayerPrefs.GetInt("W1Damage");
		tmpW1Damage += 1;
		
		PlayerPrefs.SetInt("W1Damage", tmpW1Damage);

		totalPoints -= 1;
	}
	
	public void addW1FireRate () {
		int tmpW1FireRate = PlayerPrefs.GetInt("W1FireRate");
		tmpW1FireRate += 1;

		PlayerPrefs.SetInt("W1FireRate", tmpW1FireRate);
		
		totalPoints -= 1;
	}
	
	public void addW1Range () {
		int tmpW1Range = PlayerPrefs.GetInt("W1Range");
		tmpW1Range += 1;
		
		PlayerPrefs.SetInt("W1Range", tmpW1Range);

		totalPoints -= 1;
	}
	
	// weapon 2
	public void addW2Damage () {
		int tmpW2Damage = PlayerPrefs.GetInt("W2Damage");
		tmpW2Damage += 1;
		
		PlayerPrefs.SetInt("W2Damage", tmpW2Damage);
		
		totalPoints -= 1;
	}
	
	public void addW2FireRate () {
		int tmpW2FireRate = PlayerPrefs.GetInt("W2FireRate");
		tmpW2FireRate += 1;

		PlayerPrefs.SetInt("W2FireRate", tmpW2FireRate);

		totalPoints -= 1;
	}
	
	public void addW2Range () {
		int tmpW2Range = PlayerPrefs.GetInt("W2Range");
		tmpW2Range += 1;
		
		PlayerPrefs.SetInt("W2Range", tmpW2Range);
		
		totalPoints -= 1;
	}
	
	public void setPlayerStats () {
		
		// player
		PlayerPrefs.SetInt("PPrimaryWeapon", PPrimaryWeapon);
		PlayerPrefs.SetInt("PSecondaryWeapon", PSecondaryWeapon);
		PlayerPrefs.SetInt("PHealth", PHealth);
		PlayerPrefs.SetInt("PShield", PShield);
		PlayerPrefs.SetInt("PThrusters", PThrusters);
		PlayerPrefs.SetInt("PTurnRate", PTurnRate);
		
		// w1
		PlayerPrefs.SetInt("W1Damage", W1Damage);
		PlayerPrefs.SetInt("W1FireRate", W1FireRate);
		PlayerPrefs.SetInt("W1Range", W1Range);		
		
		// w2
		PlayerPrefs.SetInt("W2FireRate", W2FireRate);
		PlayerPrefs.SetInt("W2Damage", W2Damage);		
		PlayerPrefs.SetInt("W2Range", W2Range);
		
	}
	
	public void loadPlayerStats () {
		
		// player
		PPrimaryWeapon = PlayerPrefs.GetInt("PPrimaryWeapon");
		PSecondaryWeapon = PlayerPrefs.GetInt("PSecondaryWeapon");
		PHealth = PlayerPrefs.GetInt("PHealth");
		PShield = PlayerPrefs.GetInt("PShield");
		PThrusters = PlayerPrefs.GetInt("PThrusters");
		PTurnRate = PlayerPrefs.GetInt("PTurnRate");
		
		// w1
		W1Damage = PlayerPrefs.GetInt("W1Damage");
		W1FireRate = PlayerPrefs.GetInt("W1FireRate");
		W1Range = PlayerPrefs.GetInt("W1Range");		
		
		// w2
		W2FireRate = PlayerPrefs.GetInt("W2FireRate");
		W2Damage = PlayerPrefs.GetInt("W2Damage");		
		W2Range = PlayerPrefs.GetInt("W2Range");		
		
	}
	
	
	public void resetStats () {
		
		PPrimaryWeapon = 0;
		PSecondaryWeapon = 0;
		PHealth = 0;
		PShield = 0;
		PThrusters = 0;
		PTurnRate = 0;
		
		
		// primary weapon
		W1Damage = 0;
		W1FireRate = 0;
		W1Range = 0;
		
		// secondary weapon
		W2Damage = 0;
		W2FireRate = 0;
		W2Range = 0;
		
		totalPoints = 14;
		
		setPlayerStats();
	}
	
}
