using System;
using System.Collections.Generic;
using System.Linq;
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
                    first_vowel_groups=SetVowelGroups(first_word);
                    for (int i = 1; i < words.Count(); i++)
                    {
                        string second_word = words[i];
                        if (firstWordRhymesWithSecondWord(first_word,second_word))
                        {
                            result.Add(first_word + " - " + second_word);
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
        private bool firstWordRhymesWithSecondWord(string first_word, string second_word)
        { 
            List<char> first_word_split = first_word.ToList();
            List<char> second_word_split = second_word.ToList();
            //Rule3
            if (first_word.ToLower().EndsWith(second_word.ToLower())||second_word.ToLower().EndsWith(first_word.ToLower()))
            {
                return false;
            }
            //Rule2
            int[] second_vowel_groups = SetVowelGroups(second_word);
            if (NotSameVowelGroups(second_vowel_groups, first_word, second_word))
            {
                return false;
            }
            if (first_word.Length - first_vowel_groups[1]< first_vowel_groups[1]|| second_word.Length - second_vowel_groups[1] < second_vowel_groups[1])
            {
                return false;
            }
            //Rule1
            int count_to;
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
                    if (IsAVowel(first_word_split[i].ToString()) && !IsAVowel(first_word_split[i + 1].ToString()) && 
                        IsAVowel(second_word_split[i].ToString()) && !IsAVowel(second_word_split[i + 1].ToString()))
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
                    
                    return false;
                }
            }
            return false;
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
        private int[] SetVowelGroups(string word)
        {
            int[] vowel_group = new int[2];
            vowel_group[0] = 0;
            vowel_group[1] = 0;
            char[] word_split = word.ToCharArray();
            for (int i = word.Length - 1; i > -1; i--)
            {
                while (IsAVowel(word_split[i].ToString()))
                {
                    i -= 1;
                    if (i <= 0 || !IsAVowel(word_split[i].ToString())) 
                    {
                        vowel_group[0]++;
                        vowel_group[1] = i + 1;
                        if (vowel_group[0] == 2)
                        {
                            return vowel_group;
                        }
                        if (i < 0)
                        {
                            return vowel_group;
                        }
                    }
                }
            }
            return vowel_group;
        }
        private bool NotSameVowelGroups(int[] second_vowel_groups, string first_word, string second_word)
        {
            return first_vowel_groups[0] >= 2 && second_vowel_groups[0] < 2 || second_vowel_groups[0] >= 2 && first_vowel_groups[0] < 2 || 
                first_vowel_groups[0] < 2 && second_vowel_groups[0] < 2 && first_vowel_groups[0] != second_vowel_groups[0] || 
                first_word.Length-first_vowel_groups[1] != second_word.Length-second_vowel_groups[1];
        }
    }
}