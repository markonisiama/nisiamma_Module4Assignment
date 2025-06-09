using System.Text;
using Microsoft.Maui;

namespace nisiamma_Module4Assignment;

public partial class BmiCalculator : ContentPage
{
    enum Gender { None, Male, Female }
    Gender selectedGender = Gender.None;

    public BmiCalculator()
    {
        InitializeComponent();
        UpdateSliders();
    }

    void OnMaleTapped(object sender, EventArgs e)
    {
        selectedGender = Gender.Male;
        MaleFrame.BorderColor = Colors.Blue;
        FemaleFrame.BorderColor = Colors.Transparent;
    }

    void OnFemaleTapped(object sender, EventArgs e)
    {
        selectedGender = Gender.Female;
        FemaleFrame.BorderColor = Colors.Pink;
        MaleFrame.BorderColor = Colors.Transparent;
    }

    void OnSliderValueChanged(object sender, ValueChangedEventArgs e)
    {
        UpdateSliders();
    }

    void UpdateSliders()
    {
        WeightLabel.Text = $"{Math.Round(WeightSlider.Value)} lb";
        HeightLabel.Text = $"{Math.Round(HeightSlider.Value)} in";
    }

    async void OnCalculateClicked(object sender, EventArgs e)
    {
        // Don't calculate if no gender or zero height
        if (selectedGender == Gender.None || HeightSlider.Value == 0)
            return;

        double weight = WeightSlider.Value;
        double height = HeightSlider.Value;
        double bmi = (weight / (height * height)) * 703;
        string bmiText = $"{bmi:F1}";

        // Determine status
        string status;
        if (selectedGender == Gender.Male)
        {
            if (bmi < 18.5) status = "Underweight";
            else if (bmi < 25) status = "Normal weight";
            else if (bmi < 30) status = "Overweight";
            else status = "Obese";
        }
        else
        {
            if (bmi < 18) status = "Underweight";
            else if (bmi < 24) status = "Normal weight";
            else if (bmi < 29) status = "Overweight";
            else status = "Obese";
        }

        // Recommendations (you can expand these into multiple bullets)
        string[] recs;
        switch (status)
        {
            case "Underweight":
                recs = new[]
                {
                "Increase calorie intake with nutrient-rich foods (e.g., nuts, lean protein, whole grains).",
                "Incorporate strength training to build muscle mass.",
                "Consult a nutritionist if needed."
            };
                break;

            case "Normal weight":
                recs = new[]
                {
                "Maintain a balanced diet with proteins, healthy fats, and fiber.",
                "Stay physically active with at least 150 minutes of exercise per week.",
                "Keep regular check-ups to monitor overall health."
            };
                break;

            case "Overweight":
                recs = new[]
                {
                "Reduce processed foods and focus on portion control.",
                "Engage in regular aerobic exercises (e.g., jogging, swimming) and strength training.",
                "Drink plenty of water and track your progress."
            };
                break;

            default: // "Obese"
                recs = new[]
                {
                "Consult a doctor for personalized guidance.",
                "Start with low-impact exercises (e.g., walking, cycling).",
                "Follow a structured weight-loss meal plan and consider behavioral therapy for lifestyle changes.",
                "Avoid sugary drinks and maintain a consistent sleep schedule."
            };
                break;
        }

        // Build the message
        var message = new StringBuilder()
            .AppendLine($"Gender: {selectedGender}")
            .AppendLine($"BMI: {bmiText}")
            .AppendLine($"Health Status: {status}")
            .AppendLine("Recommendations:");
        foreach (var r in recs)
            message.AppendLine($"- {r}");

        // Show popup
        await DisplayAlert(
            "Your calculated BMI results are:",
            message.ToString().TrimEnd(),
            "OK");
    }
}