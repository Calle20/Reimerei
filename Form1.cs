﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Reimerei
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            openFileDialog.Filter = "txt files (*.txt)|*.txt";
        }
        public string[] vowels = {"a", "e", "i", "o", "u", "ä", "ö", "ü"};
        public int[] first_vowel_groups;
        private void btnOpen_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                List<string> result = new List<string>();
                string filePath = openFileDialog.FileName;
                string[] lines = File.ReadAllLines(filePath);
                List<string> words = lines.ToList();
                while(words.FirstOrDefault()!=null){
                    string first_word = words.First();
                    first_vowel_groups=Set_Vowel_Groups(first_word);
                    for (int i = 1; i < words.Count(); i++)
                    {
                        string second_word = words[i];
                        if (firstWordRhymesWithSecondWord(first_word,second_word))
                        {
                            result.Add(first_word + " - " + second_word);
                            //Liste mit Reimwörter zu einem Wort
                        }
                    }
                    words.Remove(first_word);
                }
                if (result.Count==0)
                {
                    result.Add("Kein Reimpaar gefunden");   
                }
                listBox1.DataSource = result;
                
            }
        }
        private int[] Set_Vowel_Groups(string word)
        {
            int[] vowel_group =new int[2];
            vowel_group[0] = 0;
            vowel_group[1] = 0;
            char[] word_split = word.ToCharArray();
            for (int i=word.Length-1; i > 0; i--)
            {
                while (IsAVowel(word_split[i].ToString()))
                {
                    i -= 1;
                    if (!IsAVowel(word_split[i].ToString()))
                    {
                        vowel_group[0]++;
                        if (vowel_group[0] == 2)
                        {
                            vowel_group[1]=i+1;
                        }
                    }
                }
            }
            return vowel_group;
        }

        private bool firstWordRhymesWithSecondWord(string first_word, string second_word)
        { 
            List<char> first_word_split = first_word.ToList();
            List<char> second_word_split = second_word.ToList();
            #region Rule3
            if (first_word.EndsWith(second_word)||second_word.EndsWith(first_word))
            {
                return false;
            }
            #endregion
            #region Rule2
            int[] second_vowel_groups = Set_Vowel_Groups(second_word);
            if (first_word.Length - first_vowel_groups[1]< first_vowel_groups[1]|| second_word.Length - second_vowel_groups[1] < second_vowel_groups[1])
            {
                return false;
            }
            #endregion
            int count_to;
            #region Rule1
            if (first_word_split.Count > second_word_split.Count)
            {
                count_to=second_word_split.Count();
            }
            else
            {
                count_to=first_word_split.Count();
            }
            first_word_split.Reverse();
            second_word_split.Reverse();
            int vowel_groups = 0;
            for (int i=0; i < count_to; i++)
            {
                if (first_word_split[i] == second_word_split[i])
                {
                    if (IsAVowel(first_word_split[i].ToString())&& !IsAVowel(first_word_split[i+1].ToString()))
                    {
                        vowel_groups++;
                    }
                    if (first_vowel_groups[0] >= 2 && vowel_groups == 2 || first_vowel_groups[0] == 1 && vowel_groups == 1)  
                    {
                        return true;
                    }
                }
                else
                {
                    if (first_vowel_groups[0] >=2 || first_vowel_groups[0]==1)
                    {
                        return false;
                    }
                }
            }
            return true;
            #endregion

        }
        private bool IsAVowel(string letter)
        {
            if(vowels.Contains(letter.ToLower()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
