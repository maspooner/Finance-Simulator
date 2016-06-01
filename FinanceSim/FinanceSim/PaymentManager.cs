﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanceSim {
	static class PaymentManager {
		private static Random rand = new Random();
		//statics
		private static DateTime RandomDay(int month = 1, int year = 1) {
			while(month > 12) {
				month -= 12;
			}
			return new DateTime(year, month, rand.Next(1, 28));
		}
		internal static List<Payment> GeneratePayments(Profile profile) {
			//TODO add payments
			//TODO apply challenge level
			List<Payment> payments = new List<Payment>();
			//pay
			DateTime tuesday = new DateTime(profile.DesiredDate.Year, profile.DesiredDate.Month, 1);
			while (!tuesday.DayOfWeek.Equals(DayOfWeek.Tuesday))
				tuesday = tuesday.AddDays(1);
			payments.Add(new CertainFixedPayment("Paycheck", new Description("Your bi-weekly paycheck is here."), "Income", -1 * profile.BiPay, Frequency.BI_MONTHLY, tuesday));
			//other monthlies
			foreach (CertainFixedPayment cfp in profile.OtherMonthly)
				payments.Add(cfp);
			//utilities
			payments.Add(new CertainMonthDepPayment("Heating Bill", new Description("Your heating bill is due."), "Utility", rand, new decimal[] {
				97, 92, 75, 60, 30, 0, 0, 0, 25, 40, 75, 90
			}, new decimal[] {
				15, 15, 10, 10, 3, 0, 0, 0, 5, 10, 10, 15
			}, Frequency.MONTHLY_DAY, RandomDay()));
			payments.Add(new CertainMonthDepPayment("Electricity Bill", new Description("Your electricity bill is due."), "Utility", rand, new decimal[] {
				70, 60, 50, 50, 50, 55, 80, 80, 50, 50, 50, 60, 70, 60, 50
			}, new decimal[] {
				10, 5, 5, 5, 5, 10, 15, 15, 10, 5, 5, 10, 10, 5, 5
			}, Frequency.MONTHLY_DAY, RandomDay()));
			payments.Add(new CertainMonthDepPayment("Water Bill", new Description("Your water bill is due."), "Utility", rand, new decimal[] {
				20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20
			}, new decimal[] {
				7, 7,7,7,7,7,7,7,7,7,7,7
			}, Frequency.MONTHLY_DAY, RandomDay()));
			//car
			payments.Add(new RelativeRandomPayment("Car Insurance", new Description("Bi-annual car insurance premium."), "Car", 462.67m, 551.23m, rand, RandomDay(profile.DesiredDate.Month + 2, profile.DesiredDate.Year), 180));
			payments.Add(new CertainFixedPayment("Tires", new Description("Looks like you need new tires!"), "Car", 875m, Frequency.YEARLY, RandomDay(profile.DesiredDate.Month + 3, profile.DesiredDate.Year)));
			payments.Add(new CertainFixedPayment("Registration", new Description("Pay for car registration this month."), "Car", (decimal)((profile.CarYears < 5 ? 1.5 - .24 * profile.CarYears : .25) * (double)profile.CarValue / 100.0 + 50), Frequency.YEARLY, new DateTime(profile.DesiredDate.Year, profile.Birthday.Month, 1)));
			//TODO
			//food
			payments.Add(new UncertainRandomPayment("Food", new Description("You need this to survive."), "Food", Frequency.WEEKLY, 75m, 95m, rand, 1, 1, profile.DesiredDate, 2));
			payments.Add(new UncertainRandomPayment("Meal Out", new Description("You enjoy a lunch out with friends."), "Food", Frequency.MONTHLY_DAY, 6.5m, 15.5m, rand, 2, 3, profile.DesiredDate));
			payments.Add(new UncertainRandomPayment("Meal Out", new Description("You enjoy a dinner out with friends."), "Food", Frequency.MONTHLY_DAY, 9.5m, 29.5m, rand, 1, 2, profile.DesiredDate));
			payments.Add(new UncertainRandomPayment("Snack", new Description(
				"Satisfy a snack craving.",
				"Buy something sweet.",
				"Get a snack to make your hunger go away.",
				"Treat yourself to a yummy snack",
				"Time for a snack!"
				), "Food", Frequency.WEEKLY, 1m, 3.95m, rand, profile.SnackFreq / 2, profile.SnackFreq, profile.DesiredDate));
			payments.Add(new UncertainRandomPayment("Coffee", new Description(
				(payment, max) => payment > 2.1m ? 0 : 1,
                "Yum! Coffee and a morning treat!",
				"Start the day with some coffee."
				), "Food", Frequency.WEEKLY, 1.5m, 3.95m, rand, profile.CoffeeFreq / 2, profile.CoffeeFreq, profile.DesiredDate));
			//spending $
			//TODO
			//medical
			payments.Add(new RelativeRandomPayment("Dentist", new Description("Get your teeth cleaned with a visit to the dentist."), "Medical", 69.5m, 139.5m, rand, RandomDay(profile.DesiredDate.Month + 1, profile.DesiredDate.Year), 180));
			payments.Add(new UncertainFixedPayment("Co-pay", new Description("You had to co-pay at doctor's office."), "Medical", 30m, Frequency.YEARLY, 1, 1, profile.DesiredDate, rand));
			payments.Add(new UncertainFixedPayment("Co-pay", new Description("You had to co-pay at specialist's office."), "Medical", 40m, Frequency.YEARLY, 1, 1, profile.DesiredDate, rand));
			payments.Add(new UncertainAlternatingPayment("Medical Bill", "Medical", Frequency.YEARLY, 2, new decimal[] { 140, 187, 247, 298 }, new string[] {
				"You have blood tests done",
				"You have x-rays done",
				"You have lab tests done",
				"You visit the hospital"
			}, profile.DesiredDate, rand));
			//bad stuff
			payments.Add(new UncertainAlternatingPayment("Bad stuff", "Very Bad Stuff", Frequency.YEARLY, 2, new decimal[] {
				215, 219, 225, 240, 243, 299, 280, 300, 310, 368, 199, 450, 375, 675, 650, 900, 1654, 2365, 1000, 4420
			}, new string[] {
				"Buy a bike, canoe, or kayak rack for your car.",
				"Buy a dehumidifier because the moisture is ruining your stuff.",
				"Computer repairs.At least it's cheaper than a new one.",
				"A tire was shredded by a piece of metal.Buy a new pair.",
				"You lost your wallet. Subtract a total of $243 from somewhere in your budget.",
				"Your went in the ocean with your cell phone.Replace it.Minimum cost shown.",
				"You accidentally put a hole in the wall and have to pay for repairs.",
				"Somone backed over your bicylce.Spend at least the amount shown for a new one.",
				"Your game console is toast.Use the correct price, not the one shown.",
				"You backed over a neighbor's bicycle. Buy a replacement, please.",
				"Have a pet? It just got injured / sick, and guess who gets the vet bill?",
				"Television is fried.Buy a new one.The price shown is a minimun.",
				"You are a groomsman or bridesmaid. Travel to wedding, and stay in hotel.",
				"Travel across the country for family issues.",
				"You cracked a tooth and need a crown.The amount shown is your part of the bill.",
				"Your laptop stopped working.Find a replacement.The price listed is a minimum.",
				"You got in a small accident.You need to pay the $500 dedcutible.",
				"Someone backed into your car in a lot.Pay the $500 deductible.",
				"Your bicycle or laptop was stolen. (Pick one.) Enter the cost of replacement.",
				"You blew an engine block in your vehicle.Sorry."
			}, profile.DesiredDate, rand));
			payments.Add(new UncertainAlternatingPayment("Bad stuff", "Medium Bad Stuff", Frequency.YEARLY, 5, new decimal[] {
				63, 65, 72, 74, 78, 79, 80, 84, 95, 96, 97, 98, 119, 125, 126, 145, 155, 157, 179, 180, 195, 196
			}, new string[] {
				"Pay to go to high school reunion.",
				"You desperately need new underwear.",
				"Flip a coin once. If it lands heads, you have decided to get a tattoo.",
				"Buy new tools and/or a toolbox.",
				"Your favorite pants ripped and you need to replace them. You have a date!",
				"Buy a new chain for your bicycle. You have a trip with friends next weekend.",
				"Replace television remote.",
				"Buy a power drill to do projects.",
				"Replace cordless home phone set. ",
				"Throw a dinner party. Really do it up!",
				"Buy a small amountof firewood for your fireplace. Romantic dinners here we come.",
				"You need new shoes for work.",
				"Friends want to go camping.You will need a sleeping bag.",
				"You locked yourself out and lost your key.Pay a locksmith.",
				"Rent a tux(or buy a dress) for a formal event.",
				"Sink clogs and requires plummer.", 
				"You were playing indoor football and accidentally broke a window. Have it fixed.",
				"Repaint a room.",
				"You need a new vacuum cleaner because the other one is smoking.",
				"You dropped your cell phone in the toilet before your plan provided for a new one.",
				"You sat on your glasses and destroyed them.Buy new ones.",
				"Pay for this speeding ticket, and increase your auto insurance by 10 %."
			}, profile.DesiredDate, rand));
			payments.Add(new UncertainAlternatingPayment("Bad stuff", "Small Bad Stuff", Frequency.MONTHLY_DAY, 1, new decimal[] {
				8, 9, 14, 15, 17, 18, 19, 20, 21, 28, 45, 27, 24, 45, 46, 47, 48, 49, 50, 51
			}, new string[] {
				"You need light bulbs.",
				"Buy a new pack of batteries.",
				"You need ant traps before they carry away your furniture.",
				"A friend's pet peed on your carpet. Buy materials to remove the scent.",
				"Buy an umbrella.",
				"Buy new filters for your vacuum cleaner.",
				"Fill tank on gas grill.",
				"Buy a pair of water bottles for work.",
				"You need a new toaster.",
				"Your headphones were stolen.Replace them.",
				"You need a new coffee maker.You choose it.Spend at least the amount shown.",
				"Send flowers to the funeral of a friend's parent.",
				"Buy flowers for a friend in the hospital.",
				"Buy either a decoration or something for the kitchen.",
				"You left your iPod charger on a trip and need a new one.",
				"Buy devices to remove the mice which seem to outnumber you in the home.",
				"Buy holiday decorations so that your parents are impressed when they visit.",
				"Buy a toy for a holiday toy drive.",
				"Woops - parking ticket.",
				"Your sheets are old and grungy.Replace them.Find an actual cost."
			}, profile.DesiredDate, rand));
			//good stuff
			payments.Add(new UncertainAlternatingPayment("Good stuff", "Good Stuff", Frequency.YEARLY, 5, new decimal[] {
				-75, -81, -5, -100, -41, -10
			}, new string[] {
				"A friend paid you for helping with a project.",
				"You won a 50 - 50 raffle.",
				"You found some money on the streets.Yay for you.",
				"You won a scratch ticket.Woopee!",
				"You cashed in your change jar.",
				"You found some money you didn't know you had in a drawer."
			}, profile.DesiredDate, rand));
			Description birthdayDesc = new Description(
				"Happy Birthday, " + profile.FirstName + ". Your Mom has given you some money.",
				"Happy Birthday, " + profile.FirstName + ". Your Dad has given you some money.",
				"Happy Birthday, " + profile.FirstName + ". Your Uncle Henry has given you some money.",
				"Happy Birthday, " + profile.FirstName + ". Your Grandpa Gerswhin has given you some money.",
				"Happy Birthday, " + profile.FirstName + ". Your Grandma Lois has given you some money.",
				"Happy Birthday, " + profile.FirstName + ". Your Uncle Ferdinand has given you some money.",
				"Happy Birthday, " + profile.FirstName + ". Your Aunt Florence has given you some money.",
				"Happy Birthday, " + profile.FirstName + ". Your Uncle Bertram has given you some money.",
				"Happy Birthday, " + profile.FirstName + ". Your Aunt Michele has given you some money.",
				"Happy Birthday, " + profile.FirstName + ". Your Uncle Wayne has given you some money.",
				"Happy Birthday, " + profile.FirstName + ". Your Grandma Bella has given you some money.",
				"Happy Birthday, " + profile.FirstName + ". Your Grandpa Paul has given you some money.",
				"Happy Birthday, " + profile.FirstName + ". Your Aunt Emma has given you some money.",
				"Happy Birthday, " + profile.FirstName + ". Your Uncle Christian has given you some money.",
				"Happy Birthday, " + profile.FirstName + ". Your Mr. I has given you some money."
				);
            payments.Add(new CertainRandomPayment("Birthday Money", birthdayDesc, "Good stuff", -1m, -100m, rand, Frequency.YEARLY, profile.Birthday));
			payments.Add(new CertainRandomPayment("Birthday Money", birthdayDesc, "Good stuff", -1m, -100m, rand, Frequency.YEARLY, profile.Birthday));
			payments.Add(new CertainRandomPayment("Christmas Money", new Description(
				"Happy Holidays, " + profile.FirstName + ". Your Mom has given you some money.",
				"Happy Holidays, " + profile.FirstName + ". Your Dad has given you some money.",
				"Happy Holidays, " + profile.FirstName + ". Your Uncle Henry has given you some money.",
				"Happy Holidays, " + profile.FirstName + ". Your Grandpa Gerswhin has given you some money.",
				"Happy Holidays, " + profile.FirstName + ". Your Grandma Lois has given you some money.",
				"Happy Holidays, " + profile.FirstName + ". Your Uncle Ferdinand has given you some money.",
				"Happy Holidays, " + profile.FirstName + ". Your Aunt Florence has given you some money.",
				"Happy Holidays, " + profile.FirstName + ". Your Uncle Bertram has given you some money.",
				"Happy Holidays, " + profile.FirstName + ". Your Aunt Michele has given you some money.",
				"Happy Holidays, " + profile.FirstName + ". Your Uncle Wayne has given you some money.",
				"Happy Holidays, " + profile.FirstName + ". Your Grandma Bella has given you some money.",
				"Happy Holidays, " + profile.FirstName + ". Your Grandpa Paul has given you some money.",
				"Happy Holidays, " + profile.FirstName + ". Your Aunt Emma has given you some money.",
				"Happy Holidays, " + profile.FirstName + ". Your Uncle Christian has given you some money.",
				"Happy Holidays, " + profile.FirstName + ". Your Mr. I has given you some money."
				), "Good stuff", -50m, -200m, rand, Frequency.YEARLY, new DateTime(profile.DesiredDate.Year, 12, 25)));
			//misc
			//TODO
			return payments;
		}
	}
}
