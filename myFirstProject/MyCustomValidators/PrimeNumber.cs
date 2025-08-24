using System;  

using System.ComponentModel.DataAnnotations;  

  
namespace myFirstProject.MyCustomValidators; 
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]  

public class PrimeNumberValidatorAttribute : ValidationAttribute  

{  

    public bool ShouldBePrime { get; set; } = true; // Default check for prime  

  

    public PrimeNumberValidatorAttribute() { }  

  

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)  

    {  

        if (value == null)  

            return ValidationResult.Success; // or handle nulls as invalid, based on your needs  

  

        if (int.TryParse(value.ToString(), out int number))  

        {  

            bool isPrime = IsPrime(number);  

  

            if (ShouldBePrime && isPrime)  

                return ValidationResult.Success;  

            if (!ShouldBePrime && !isPrime)  

                return ValidationResult.Success;  

  

            string message = ShouldBePrime  

                ? $"{validationContext.DisplayName} must be a prime number."  

                : $"{validationContext.DisplayName} must not be a prime number.";  

  

            return new ValidationResult(message);  

        }  

        else  

        {  

            return new ValidationResult($"{validationContext.DisplayName} must be a valid integer.");  

        }  

    }  

  

    private bool IsPrime(int number)  

    {  

        if (number < 2) return false;  

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