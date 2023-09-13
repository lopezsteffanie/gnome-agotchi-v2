
# Gnome-agotchi V2

This is a working update to my original Capstone Project that I created during my time at Ada Developers Academy.



## 🔗 Links

[![originalProject](https://img.shields.io/badge/original_project-100000?style=for-the-badge&logo=github&logoColor=white)](https://github.com/lopezsteffanie/Gnome-agotchi)

## 💡 Features

- Cross platform for iOS & Android
- Signup/Login/Logout with Firebase Authentication
- Save user data using Realtime Database
- Background changes with local time

### Gnome Randomization
- Random gnome color generated at start of the game, choice between:
  - 🔴red
  - 🟠orange
  - 🟡yellow
  - 🟢green
  - 🔵blue
  - 🟣purple
  - 💖pink
- Random gnome peronality generated at the start of the game, which affects gnome needs:
  - **Friendly:** This gnome is warm, welcoming, and always ready to help others. They make friends easily and are known for their hospitality.
    - **Skill:** *Socializing -* Increases the pet's happiness when interacting with other pets.
    - **Ability:** *Comforter -* Can calm down anxious or upset pets.
    - **Trait:** *Sociable -* Enjoys playing with other pets and often seeks companionship. 
  - **Grumpy:** The grumpy gnome is a bit irritable and often complains about things. However, they have a good heart deep down and can be quite locable once you get to know them.
    - **Skill:** *Independence -* Prefers solitude and may become upset with too much attention.
    - **Ability:** *Resilience -* Less affected by negative mood changes.
    - **Trauit:** *Self-Sufficient -* Doesn't rely on others for happiness and can entertain itself.
  - **Adventurous:** This gnome loves exploring and taking risks. They are always up for an adventure and enjoy discovering new places and treasures.
    - **Skill:** *Exploration -* Encourages the pet to explore its environment.
    - **Ability:** *Fearless Companion -* Boosts the pet's courage when faced with new experiences.
    - **Trait:** *Energetic -* Loves going on adventures and trying new activities.
  - **Bookworm:** The bookworm gnome is highly intelligent and spends most of their time reading and studying. They are a valueable source of knowledge and wisdom.
    - **Skill:** *Education -* Accelerates the pet's learning rate.
    - **Ability:** *Knowledge Share -* Teaches the pet new tricks or skills faster.
    - **Trait:** *Intellectual -* Prefers spending time learning and reading.
  - **Shy:** Shy gnomes are introverted and often prefer the company of a few close friends. They can be quiet and reserved but are very loyal.
    - **Skill:** *Trust Building -* Helps shy pets build trust with the player.
    - **Ability:** *Comfort Zone -* Creates a safe space for shy pets to relax.
    - **Trait:** *Timid -* Requires patience and gentle care to come out of its shell.
  - **Funny:** These gnomes have a great sense of humor and love making others laugh. They are the laife of the party and always have a joke or funny story to share.
    - **Skill:** *Entertainment -* Makes the pet laugh and boosts its mood.
    - **Ability:** *Clowning Around -* Performs funny tricks or acts to entertain the pet.
    - **Trait:** *Playful -* Loves engaging in playful activities and jokes.
  - **Mysterious:** Mysterious gnomes are enigmatic and keep to themselves. They often have hidden talents or secrets that are revealed over time.
    - **Skill:** *Curiosity -* Encourages the pet to investigate and discover hidden items.
    - **Ability:** *Enigmatic Charm -* Adds an air of mystery to the pet's personality.
    - **Trait:** *Intriguing -* Captivates other pets with its enigmatic presence.
  - **Nature-Loving:** These gnomes have a deep connection with nature and are often found in gardens or forests. They have a green thumb and love caring for plants and animals.
    - **Skill:** *Outdoor Skills -* Enhances the pet's abilities when in natural surroundings.
    - **Ability:** *Harmony with Nature -* Calms the pet when in natural environments.
    - **Trait:** *Environmentalist -* Enjoys spending time in gardens and outdoor spaces.
  - **Creative:** Creative gnomes are artists and inventors. They are constantly coming up with new ideas and enjoy crafting and building.
    - **Skill:** *Artistic Expression -* Encourages the pet to create artwork or crafts.
    - **Ability:** *Masterpiece -* Boosts the quality of the pet's creative endeavors.
    - **Trait:** *Artistic -* Finds joy in artistic activities and self-expression.
  - **Loyal:** Loyalty is the most important trait for these gnomes. THey will stand by their friends through thick and thin and can be counted on in times of need.
    - **Skill:** *Loyalty -* Deepens the bond between the player and the pet.
    - **Ability:** *Protector -* Increases the pet's sense of security and safety.
    - **Trait:** *Devoted -* Remains loyal and committed to the player.
  - **Curious:** Curious gnomes are always asking questions and seeking knowledge. They have childlike wonder about the world and are fascinated by everything.
    - **Skill:** *Exploration -* Encourages the pet to explore its environment and discover hidden items.
    - **Ability:** *Inquisitive Mind -* Boosts the pet's curiosity and desire to investigate.
    - **Trait:** *Curious Nature -* Always eager to learn and discover new things.
  - **Stubborn:** Stubborn gnomes are determined and never give up easily. They can be a bit obstinate at times but are incredibly persistent.
    - **Skill:** *Determination -* Helps the pet persist in training and activities, even when faced with challenges.
    - **Ability:** *Tenacious Spirit -* Refuses to give up easily and inspires the pet to keep trying.
    - **Trait:** *Stubborn Streak -* Can be hard-headed and resistant to change.
  - **Wise:** Wise gnomes are the sages of the gnome world. They provide guidance and advice to other gnomes and are highly respected.
    - **Skill:** *Wisdom -* Imparts valuable life lessons to the pet, increasing its intelligence.
    - **Ability:** *Sage Advice -* Provides guidance and solutions when the pet faces problems.
    - **Trait:** *Wise Counselor -* Offers insights and thoughtful perspectives.
  - **Helpful:** These gnomes are always looking for ways to assist others. They are generous and selfless, often putting the needs of others before their own.
    - **Skill:** *Assistance -* Assists the player in pet care tasks, making them more efficient.
    - **Ability:** *Supportive Aura -* Calms and comforts the pet when it's distressed.
    - **Trait:** *Helpful Nature -* Always willing to lend a hand and be of service.
  - **Mischievous:** Mischevous gnomes enjoy playing pranks and tricks on their fellow gnomes. They keep things lighthearted and fun.
    - **Skill:** *Playfulness -* Engages the pet in fun and playful activities, boosting its happiness.
    - **Ability:** *Prankster -* Pulls harmless pranks that amuse and entertain the pet.
    - **Trait:** *Mischievous Spirit -* Loves mischief and playfully teases other pets.
### Gnome needs
  - **Hunger:** Pets need to be fed regularly. Players must provide food to keep their pets from getting hungry and malnourished.
  - **Thirst:** Like hunger, pets also need water to stay hydrated. Players should provide access to clean water resources.
  - **Energy:** Pets require rest and sleep. Players need to ensure their pets get enough sleep to maintain their energy levels.
  - **Hygiene:** Pets can get dirty over time. Players should groom or clean their pets to keep them clean and healthy.
  - **Social Interaction:** Most pets are social animals. They need attention, affection, and interaction with their owners or other pets in the game.
  - **Exercise:** Pets often need physical activity to stay fit and happy. Players may need to engage in activities or play games with their pets.
  - **Mental Stimulation:** Some pets require mental stimulation, such as puzzles or toys, to prevent boredom and keep them mentally active.
  - **Healthcare:** Pets can get sick or injured. Players should provide healthcare, such as medication or visits to a virtual veterinarian.
  - **Affection:** Pets thrive on love and affection from their owners. Players should show their pets love and care.
  - **Environment:** The pet's living environment should be clean and comfortable. Players can customize and improve the pet's habitat.
  - **Training:** Training needs can include teaching pets tricks, commands, or behaviors. Successful training can lead to improved obedience and happiness.
  - **Emotional Needs:** Pets may have emotional needs such as companionship, security, or stimulation. Players should attend to these needs to keep their pets emotionally balanced.
  - **Recreation:** Pets might enjoy recreational activities like playing with toys, watching TV, or listening to music.
  - **Exploration:** Some pets have a curiosity for exploration. Players can provide opportunities for pets to explore new areas or objects.
  - **Career or Hobby:** In more complex pet simulators, pets might have career aspirations or hobbies that players can help them pursue.

## ✅ Credits

This project uses the following assets:
- [Complete GUI Essential Pack](https://crusenho.itch.io/complete-gui-essential-pack) created by [Crusenho](https://crusenho.itch.io/)
- [Icons Essential Pack](https://crusenho.itch.io/icons-essential-pack-free-icons) created by [Crusenho](https://crusenho.itch.io/)
- All assets are licensed under the [Creative Commons Attribution 4.0 International License (CC BY 4.0)](https://creativecommons.org/licenses/by/4.0/)

- *[Simple Sky Pixel Backgrounds](https://caniaeast.itch.io/simple-sky-pixel-backgrounds) created by [Cania](https://caniaeast.itch.io/) is licensed under the [Creative Commons Attribution-NonCommercial 4.0 International (CC BY-NC 4.0)](https://creativecommons.org/licenses/by-nc/4.0/)*
- *Gnome assets created by me*
