#speaker:CardMerchant
#portrait:CardMerchant
#layout:left
Hello we are doing some work now comeback later!
Nice Day!

-> main

=== main ===
What can I do for you young adventurer?
        +[Shop]
            -> END
        +[Chat]
            -> chosen("Chat")
        +[Bye]
            -> chosen("Bye")
            
=== chosen(option) ===
You chose {option}!
-> END