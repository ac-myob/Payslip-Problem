using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;


namespace payslipt {    
    public class TaxCalc2 {
        private int getGrossIncome(int salary) {
            return (int)Math.Round((double)salary/12);
        }

        private int getIncomeTax(int salary) {
            int[] taxBracket = {0, 18200, 37000, 87000, 180000};
            int[] flatTax = {0, 0, 3572, 19822, 54232};
            double[] taxRate = {0, 0.19, 0.325, 0.37, 0.45};
            int incomeTax = 0;
            const int MONTHS_IN_YEAR = 12;

            // Find correct tax bracket for salary
            for (int t=taxBracket.Length - 1; t>-1; --t) {
                if (salary > taxBracket[t]) {
                    // Income Tax = (Flat Tax + (Salary - Tax Threshold for Salary) x Tax Rate)/12 (Rounded to nearest integer)
                    incomeTax += (int)Math.Round((flatTax[t] + (salary - taxBracket[t]) * taxRate[t])/MONTHS_IN_YEAR);
                    return incomeTax;
                }
            }
            return incomeTax;
        }
        private int getNetIncome(int salary) {
            return getGrossIncome(salary) - getIncomeTax(salary);
        }

        private int getSuper(int salary, double superRate) {
            return (int)Math.Round(getGrossIncome(salary) * superRate);
        }

        private string getPayslipInfo(string userInfo) {
            // Split user info by comma delimiter
            string[] splittedUserInfo = userInfo.Split(',');
            // Evaluate and return payslip information
            string name = splittedUserInfo[0] + " " + splittedUserInfo[1];
            string period = splittedUserInfo[splittedUserInfo.Length - 1] + " - " + splittedUserInfo[splittedUserInfo.Length - 2];
            int salary = Convert.ToInt32(splittedUserInfo[2]);
            double superRate = (double)Convert.ToInt32(splittedUserInfo[3])/100;
            int grossIncome = getGrossIncome(salary);
            int incomeTax = getIncomeTax(salary);
            int netIncome = getNetIncome(salary);
            int super = getSuper(salary, superRate);

            return $"{name},{period},{grossIncome},{incomeTax},{netIncome},{super}";
        }

        public void run(string inputFilename, string outputFilename) {
            string[] lines = File.ReadAllLines(inputFilename);
            using (System.IO.StreamWriter newFile = new System.IO.StreamWriter(@outputFilename, false)) {
                // Write header to new file
                newFile.WriteLine("Name,Pay Period,Gross Income,Income Tax,Net Income,Super");
                // Start at 1 to skip header
                for (int lineNumber = 1; lineNumber < lines.Length; ++lineNumber) {
                    // Write user information to csv file
                    newFile.WriteLine(getPayslipInfo(lines[lineNumber]));
                }    
            } 
        }
    }
    public class TaxCalc {
        private string firstName;
        private int salary;
        private string lastName;
        private double super;
        private string startDate;
        private string endDate;

        // Constructor
        public TaxCalc() {}

        // Setters for user information
        private void setFirstName(string firstName) {
            this.firstName = firstName;
        }
        private void setLastName(string lastName) {
            this.lastName = lastName;
        }
        private void setSalary(int salary) {
            this.salary = salary;
        }
        private double toDecimal(int num) {
            return (double)num/100;
        }
        private void setSuper(int super) {
            this.super = toDecimal(super);
        }
        private void setStartDate(string startDate) {
            this.startDate = startDate;
        }
        private void setEndDate(string endDate) {
            this.endDate = endDate;
        }
        
        private int getGrossIncome() {
            return (int)Math.Round((double)this.salary/12);
        }

        private int getIncomeTax() {
            int[] taxBracket = {0, 18200, 37000, 87000, 180000};
            int[] flatTax = {0, 0, 3572, 19822, 54232};
            double[] taxRate = {0, 0.19, 0.325, 0.37, 0.45};
            int incomeTax = 0;
            const int MONTHS_IN_YEAR = 12;

            // Find correct tax bracket for salary
            for (int t=taxBracket.Length - 1; t>-1; --t) {
                if (this.salary > taxBracket[t]) {
                    // Income Tax = (Flat Tax + (Salary - Tax Threshold for Salary) x Tax Rate)/12 (Rounded to nearest integer)
                    incomeTax += (int)Math.Round((flatTax[t] + (this.salary - taxBracket[t]) * taxRate[t])/MONTHS_IN_YEAR);
                    return incomeTax;
                }
            }
            return incomeTax;
        }
        private int getNetIncome() {
            return getGrossIncome() - getIncomeTax();
        }

        private int getSuper() {
            return (int)Math.Round(getGrossIncome() * this.super);
        }

        private string getUserInput(string question, string regex) {
            string userResponse = "";

            do {
                Console.Write(question);
                userResponse = Console.ReadLine();
                if (!Regex.IsMatch(userResponse, regex)) {
                    Console.Write("Invalid input. Please try again.\n");
                }
            } while (!Regex.IsMatch(userResponse, regex));

            return userResponse;
        }
        private void getDetails() {
            setFirstName(getUserInput("Please input your name: ", "^[A-Z][a-z]*$"));
            setLastName(getUserInput("Please input your surname: ", "^[A-Z][a-z]*$"));
            setSalary(Convert.ToInt32(getUserInput("Please input your salary: ", "^[0-9]*$")));
            setSuper(Convert.ToInt32(getUserInput("Please input your super: ", "^[0-9]*$")));
            setStartDate(getUserInput("Please input your start date: ", "^[a-zA-Z0-9 ]*$"));
            setEndDate(getUserInput("Please input your end date: ", "^[a-zA-Z0-9 ]*$"));
        }
        private void getPayslip() {
            Console.WriteLine(String.Format("Name: {0} {1}", firstName, lastName));
            Console.WriteLine(String.Format("Pay Period: {0} - {1}", startDate, endDate));
            Console.WriteLine("Gross Income: " + getGrossIncome());
            Console.WriteLine("Income Tax: " +getIncomeTax());
            Console.WriteLine("Net Income: " + getNetIncome());
            Console.WriteLine("Super: " + getSuper());
        }
        public void run() {
            Console.WriteLine("Welcome to the payslip generator!\n");
            getDetails();
            Console.WriteLine("\nYour payslip has been generated:\n");
            getPayslip();
            Console.WriteLine("\nThank you for using MYOB!\n");
        }

        public void runCsv() {

        }
    }

    class Program {
        static void Main(string[] args) {
            TaxCalc taxCalculator = new TaxCalc();
            TaxCalc2 taxCalculator2 = new TaxCalc2();
            taxCalculator2.run("Contacts.csv","Tax.csv");
        }
    }
}