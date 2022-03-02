using System;
using System.Text.RegularExpressions;


namespace payslipt {    
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

        private string getUserInput(string question) {
            string userResponse = "";

            do {
                Console.Write(question);
                userResponse = Console.ReadLine();
                if (string.IsNullOrEmpty(userResponse)) {
                    Console.Write("Empty field detected. Please try again.\n");
                }
            } while (string.IsNullOrEmpty(userResponse));

            return userResponse;
        }
        // private bool matchRegex(string str, string regex) {
        //     if (Regex.IsMatch(str, regex)) {
		// 	Console.WriteLine("Alphanumeric String");
        //     }
        //     else {
        //         Console.WriteLine("Non-Alphanumeric String");
        //     }
        // }
        private void getDetails() {
            setFirstName(getUserInput("Please input your name: "));
            setLastName(getUserInput("Please input your surname: "));
            setSalary(Convert.ToInt32(getUserInput("Please input your salary: ")));
            setSuper(Convert.ToInt32(getUserInput("Please input your super: ")));
            setStartDate(getUserInput("Please input your start date: "));
            setEndDate(getUserInput("Please input your end date: "));
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
    }

    class Program {
        static void Main(string[] args) {
            TaxCalc taxCalculator = new TaxCalc();
            taxCalculator.run();
        }
    }
}