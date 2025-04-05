using System.Collections.Generic;

namespace Dierentuin_App.Models.AnimalDecorator
{
    // Base interface
    public interface IAnimalCharacteristic
    {
        string GetCharacteristics();
    }

    // Concrete base implementation
    public class BaseAnimalCharacteristic : IAnimalCharacteristic
    {
        private readonly Animal _animal;

        public BaseAnimalCharacteristic(Animal animal)
        {
            _animal = animal;
        }

        public virtual string GetCharacteristics()
        {
            return $"{_animal.Size}";
        }
    }

    // Base decorator class
    public abstract class CharacteristicDecorator : IAnimalCharacteristic
    {
        protected readonly IAnimalCharacteristic _characteristic;

        public CharacteristicDecorator(IAnimalCharacteristic characteristic)
        {
            _characteristic = characteristic;
        }

        public virtual string GetCharacteristics()
        {
            return _characteristic.GetCharacteristics();
        }
    }

    // Concrete decorators
    public class TrainedDecorator : CharacteristicDecorator
    {
        public TrainedDecorator(IAnimalCharacteristic characteristic) : base(characteristic) { }

        public override string GetCharacteristics()
        {
            return $"{_characteristic.GetCharacteristics()}, trained for shows";
        }
    }

    public class MedicalDecorator : CharacteristicDecorator
    {
        public MedicalDecorator(IAnimalCharacteristic characteristic) : base(characteristic) { }

        public override string GetCharacteristics()
        {
            return $"{_characteristic.GetCharacteristics()}, needs special medical care";
        }
    }

    // Manager class
    public static class AnimalCharacteristicDecorator
    {
        public static string GetDecoratedCharacteristics(Animal animal)
        {
            // Start with the base characteristics
            IAnimalCharacteristic characteristic = new BaseAnimalCharacteristic(animal);

            // Apply decorators based on animal properties
            if (animal.SecurityRequirement == Animal.AnimalSecurityRequirement.Critical)
            {
                characteristic = new MedicalDecorator(characteristic);
            }

            if (animal.Size == Animal.AnimalSize.Large)
            {
                characteristic = new TrainedDecorator(characteristic);
            }

            // Return the decorated characteristics
            return characteristic.GetCharacteristics();
        }
    }
}