﻿using System.Globalization;
using System.Web.Mvc;

namespace ImageContestSystem.Web.CustomAttributes
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Web;

    public class DateGreaterThanAttribute : ValidationAttribute, IClientValidatable 
    {
        public string FirstDate { get; private set; }
        public string SecondDate { get; private set; }
        public CompareType Compare { get; set; }

        public DateGreaterThanAttribute(string firstDate, string secondDate, CompareType type)
            : base()
        {
            FirstDate = firstDate;
            SecondDate = secondDate;
            Compare = type;
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata
            metadata, ControllerContext context)
        {
            var clientValidationRule = new ModelClientValidationRule()
            {
                ErrorMessage = FormatErrorMessage(FirstDate + " must be " +
                 Enum.GetName(typeof(CompareType), Compare) + " " + SecondDate),
                ValidationType = "datecomparer"
            };
            clientValidationRule.ValidationParameters.Add("firstdate", FirstDate);
            clientValidationRule.ValidationParameters.Add("seconddate", SecondDate);
            clientValidationRule.ValidationParameters.Add("compare", Compare);
            return new[] { clientValidationRule }; 
        }

        protected override ValidationResult IsValid(object value, ValidationContext
            validationContext)
        {
            var firstDate = validationContext.ObjectInstance.GetType().GetProperty(FirstDate);
            var secondDate = validationContext.ObjectInstance.GetType().GetProperty(SecondDate);
            var firstDateValue = firstDate.GetValue(validationContext.ObjectInstance, null);
            var secondDateValue = secondDate.GetValue(validationContext.ObjectInstance, null);
            if (value is DateTime && ((firstDateValue is DateTime) && (secondDateValue is DateTime)))
            {
                if (Compare == CompareType.EqualsTo)
                {
                    bool equals = ((DateTime) firstDateValue) == ((DateTime) secondDateValue);
                    if (!equals)
                    {
                        return new ValidationResult(FormatErrorMessage(FirstDate + " must be equal to " +
                                                                       SecondDate));
                    }
                }
                else if (Compare == CompareType.GreaterThan)
                {
                    bool equals = ((DateTime) firstDateValue) > ((DateTime) secondDateValue);
                    if (!equals)
                        return new ValidationResult(FormatErrorMessage(FirstDate + " must be greater than " +
                                                                       SecondDate));
                }
                else if (Compare == CompareType.LessThan)
                {
                    bool equals = ((DateTime) firstDateValue) < ((DateTime) secondDateValue);
                    if (!equals)
                        return new ValidationResult(FormatErrorMessage(FirstDate + " must be less than " +
                                                                       SecondDate));
                }
            }
            return ValidationResult.Success;
        }
    }
}
