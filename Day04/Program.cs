using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Day04
{
    class Program
    {
        public static string Inputs =
            $"..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}..{Path.DirectorySeparatorChar}Input{Path.DirectorySeparatorChar}";

        static void Main(string[] args)
        {
            Console.WriteLine("----------");
            Console.WriteLine("Demo:");
            DoWork(File.ReadAllLines($"{Inputs}demo_input.txt"));
            Console.WriteLine("----------");
            
            Console.WriteLine("invalid:");
            DoWork(File.ReadAllLines($"{Inputs}invalid.txt"));
            Console.WriteLine("----------");
            
            Console.WriteLine("valid:");
            DoWork(File.ReadAllLines($"{Inputs}valid.txt"));
            Console.WriteLine("----------");
            
            Console.WriteLine("Real:");
            DoWork(File.ReadAllLines($"{Inputs}input.txt"));
            Console.WriteLine("----------");
        }

        public static void DoWork(string[] lines)
        {
            var passportLines = SplitToPassportLines(lines);
            var passports = GetPassports(passportLines);
            
            {
                /*
                 *  byr (Birth Year)
                    iyr (Issue Year)
                    eyr (Expiration Year)
                    hgt (Height)
                    hcl (Hair Color)
                    ecl (Eye Color)
                    pid (Passport ID)
                    cid (Country ID)
                 */
                int valid = 0;

                foreach (var passport in passports)
                {
                    bool isvalid = passport.Keys.Contains("byr") && passport.Keys.Contains("iyr")
                                                                 && passport.Keys.Contains("eyr")
                                                                 && passport.Keys.Contains("hgt")
                                                                 && passport.Keys.Contains("hcl")
                                                                 && passport.Keys.Contains("ecl")
                                                                 && passport.Keys.Contains("pid");
                    if (isvalid) valid++;


                }
                
                Console.WriteLine(valid);
            }
            
            //part2
            {
                int valid = 0;
                foreach (var passport in passports)
                {
                    bool isvalid = passport.Keys.Contains("byr") && passport.Keys.Contains("iyr")
                                                                 && passport.Keys.Contains("eyr")
                                                                 && passport.Keys.Contains("hgt")
                                                                 && passport.Keys.Contains("hcl")
                                                                 && passport.Keys.Contains("ecl")
                                                                 && passport.Keys.Contains("pid");
                    
                    if(isvalid == false) continue;

                    // byr (Birth Year) - four digits; at least 1920 and at most 2002.
                    isvalid = passport["byr"].Length == 4;
                    if(isvalid == false) continue;
                    isvalid = int.TryParse(passport["byr"], out int birth);
                    if(isvalid == false) continue;
                    isvalid = birth switch
                        {
                            >= 1920 and <= 2002 => true,
                            _ => false
                        };
                    if(isvalid == false) continue;
                    
                    // iyr (Issue Year) - four digits; at least 2010 and at most 2020.
                    isvalid = passport["iyr"].Length == 4;
                    if(isvalid == false) continue;
                    isvalid = int.TryParse(passport["iyr"], out int issueYear);
                    if(isvalid == false) continue;
                    isvalid = issueYear switch
                        {
                            >= 2010 and <= 2020 => true,
                            _ => false
                        };
                    if(isvalid == false) continue;
                    
                    // eyr (Expiration Year) - four digits; at least 2020 and at most 2030.
                    isvalid = passport["eyr"].Length == 4;
                    if(isvalid == false) continue;
                    isvalid = int.TryParse(passport["eyr"], out int expirationYear);
                    if(isvalid == false) continue;
                    isvalid = expirationYear switch
                        {
                            >= 2020 and <= 2030 => true,
                            _ => false
                        };
                    if(isvalid == false) continue;
                    
                    // hgt (Height) - a number followed by either cm or in:
                    // If cm, the number must be at least 150 and at most 193.
                    // If in, the number must be at least 59 and at most 76.
                    {
                        var match = Regex.Match(passport["hgt"], @"([1-9][0-9]+)(cm|in)");
                        if (match.Success == false) continue;
                        (int height, string type) = (int.Parse(match.Groups[1].Value), match.Groups[2].Value);
                        if (type == "cm" && height >= 150 && height <= 193) isvalid = true;
                        else if (type == "in" && height >= 59 && height <= 76) isvalid = true;
                        else isvalid = false;
                        
                        if(isvalid == false) continue;
                    }
                    
                    // hcl (Hair Color) - a # followed by exactly six characters 0-9 or a-f.
                    {
                        var match = Regex.Match(passport["hcl"], @"#([0-9a-f]{6})");
                        if (match.Success == false) continue;
                    }

                    //ecl (Eye Color) - exactly one of: amb blu brn gry grn hzl oth.
                    isvalid = new[] { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" }.Contains(passport["ecl"]);
                    if(isvalid == false) continue;

                    //pid (Passport ID) - a nine-digit number, including leading zeroes.
                    {
                        var match = Regex.Match(passport["pid"], @"([0-9]{9})");
                        if (match.Success == false) continue;
                    }


                    if (isvalid) valid++;
                }
                //sad times, 1 false reading -> result -1 for input.txt
                Console.WriteLine(valid);

            }
            
            
            
            
        }

        private static List<Dictionary<string, string>> GetPassports(List<List<string>> passportLines)
        {
            var passports = new List<Dictionary<string, string>>();

            foreach (var passlines in passportLines)
            {
                Dictionary<string, string> passport = new Dictionary<string, string>();

                foreach (var line in passlines)
                {
                    MatchCollection matches = Regex.Matches(line, @"([a-z]+):([\w#]+)");
                    //MatchCollection matches = Regex.Matches(line, @"([a-z]+):");

                    foreach (Match match in matches)
                    {
                        passport[match.Groups[1].Value] = match.Groups[2].Value;
                    }
                }

                if (passport.Keys.Count > 0) passports.Add(passport);
            }

            return passports;
        }

        private static List<List<string>> SplitToPassportLines(string[] lines)
        {
            List<List<string>> passports = new List<List<string>>();
            int currentPass = 0;
            passports.Add(null);
            passports[currentPass] = new List<string>();
            foreach (var line in lines)
            {
                if (passports[currentPass] is null) passports[currentPass] = new List<string>();

                if (line == "")
                {
                    currentPass++;
                    passports.Add(null);
                    continue;
                }

                passports[currentPass].Add(line);
            }

            return passports;
        }
    }
}