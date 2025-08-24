using System;  

using System.ComponentModel.DataAnnotations;  

  
namespace myFirstProject.MyCustomValidators; 

public class IsPrimeNumberAttribute : ValidationAttribute  

{  

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)  

    {  

        if (value == null)  

            return ValidationResult.Success; // Or handle null if needed  

  

        // Convert to int  

        if (int.TryParse(value.ToString(), out int number))  

        {  

            if (number < 2)  

                return new ValidationResult($"{validationContext.DisplayName} must be a prime number greater than 1.");  

  

            if (IsPrime(number))  

                return ValidationResult.Success;  

            else  

                return new ValidationResult($"{validationContext.DisplayName} must be a prime number.");  

        }  

        else  

        {  

            return new ValidationResult($"{validationContext.DisplayName} must be a valid integer.");  

        }  

    }  

  

    private bool IsPrime(int number)  

    {  

        if (number == 2) return true;  

        if (number % 2 == 0) return false;  

  

        var boundary = (int)Math.Floor(Math.Sqrt(number));  

  

        for (int i = 3; i <= boundary; i += 2)  

        {  

            if (number % i == 0)  

                return false;  

        }  

  

        return true;  

    }  

} 