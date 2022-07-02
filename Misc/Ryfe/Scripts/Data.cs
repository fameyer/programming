using UnityEngine;
using System.Collections;

public static class Data{

	// phase time specifications
	private static float phase_time_single = 0.0f;
	private static float[] phase_time_multiple = {60.0f, 50.0f, 50.0f, 40.0f, 40.0f, 30.0f, 30.0f, 20.0f, 20.0f, 15.0f};

	// waiting time specifications
	private static float waiting_time_single = 0.0f;
	private static float[] waiting_time_multiple = {8.0f, 8.0f, 7.5f, 7.5f, 7.0f, 7.0f, 6.5f, 6.5f, 6.0f, 6.0f};

	private static int counter = 0;

	// bool to show single level selection (false) or multiple level structure (true)
	private static bool multi_level = true;

	// player name 
	private static string name = "John";

	// Custom input
	private static bool custom = false;
	private static float custom_phase = 50.0f;
	private static float custom_gravity = 8.0f;

	// music and sound handles
	private static bool music_on = true;
	private static bool sound_on = true;

	public static int getCounter(){
		return counter;
	}

	public static void setCounter(int i){
		counter = i;
	}

	// set and get Player Name
	public static void setName (string a){
		name = a;
	}

	public static string getName(){
		return name;
	}

	// set and get custom times
	public static void setCustomPhaseTime(float s){
		custom_phase = s;
		custom = true;
	}
	public static float getCustomPhaseTime(float s){
		return custom_phase;
	}
	public static void setCustomGravityTime(float s){
		custom_gravity = s;
		custom = true;
	}
	public static float getCustomGravityTime(float s){
		return custom_gravity;
	}

	// set and get level modes
	public static bool getMode(){
		return multi_level;
	}

	public static void setMode(bool mode){
		multi_level = mode;
	}

	public static void incrCounter (){
		++counter;
	}

	public static void setPhaseTime(float a, int i = 0){
		if (multi_level) {
			phase_time_multiple [i] = a;
		} 
		else {
			phase_time_single = a;
		}
	}

	public static void setWaitingTime(float a, int i = 0){
		if (multi_level) {
			waiting_time_multiple [i] = a;
		} 
		else {
			waiting_time_single = a;
		}
	}

	// preliminary
	public static int getFinalLevel (){
		return 9;
	}

	public static float getPhaseTime(){
		if (multi_level) {
			return phase_time_multiple [counter];
		} 
		else {
			if(!custom){
				return phase_time_single == 0.0f ? phase_time_multiple [counter] : phase_time_single;
			}
			else{
				return custom_phase;
			}
		}
	}
	
	public static float getWaitingTime(){
		if (multi_level) {
			return waiting_time_multiple [counter];
		} 
		else {
			if(!custom){
				return waiting_time_single == 0.0f ? waiting_time_multiple [counter] : waiting_time_single;
			}
			else{
				return custom_gravity;
			}
		}
	}

	// music and sound
	public static void setMusic(bool s){
		music_on = s;
	}
	public static void setSound(bool s){
		sound_on = s;
	}
	public static bool getMusic(){
		return music_on;
	}
	public static bool getSound(){
		return sound_on;
	}

	public static void restart(){
		counter = 0;
		custom = false;
	}
}