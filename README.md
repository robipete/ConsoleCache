# ConsoleCache
Console front end for a cache

RobiLabs Console Cache Demonstration Program V 0.01

Compressed with 7 zip: http://www.7-zip.org/download.html

Extension 7z doesn't usually get blocked by email, and you should be able to decompress it with any zip program.

Thought History:

So to build a cache, something we use so commonly but give such little though.

Initially I came up with a series of successively more complicated designs. I started with the thought I would need a key, value store. 

Then I would need to know in ticks perhaps starting from the time the program started to when each key was last added/accessed and keep those in an ordered list.

Well to quickly access a key, value pair efficiently by time I might need another dictionary key = ticks value = key to find the oldest element to remove in the cache dictionary.

Then I'd need a third list of actual tick values to keep in order by last addition deleting/re-adding entries as their root key value might have been accessed. Ouch.

Three layers of structures for a cache, hmm. Trying to avoid sorting anything anyway.

I didn't use a queue as accessing the middle of a queue was expensive and dictionary has O(1) access time by key. Also I wanted to just use the single data structure to keeps things really simple. 
The more I thought about it the more I didn't want layers of data structures with all that plumbing. Sure it's ultimately O(1) space, and more or less operations depending (most complex win), but...

I read about how the underlying data structures for generic lists do preserve order of adds internally. I figured I could collapse down to only the first dictionary and just keep it ordered.  

Derived requirements:

A SET command which causes an eviction should just kick the oldest (first) element out due to the ordering then the following add should be youngest (last). SET without an eviction should just go last.

A GET should move the element gotten to the end of the list making it the youngest. 
Implementation detail: I tried a delete and then add. Unfortunately, the dictionary held the place open at the site of the deletion in the underlying structure,
so the subsequent add went to the empty spot (always the first, opposite of desired behaviour) rather than the bottom in my case.

If that had worked SET with an eviction would have been O(1).

So that didn't quite work. I just wanted to delete the top of the dictionary and add at the bottom and move a middle element to the bottom fast. 
I finally had to use some linq to pull out the last n - 1 elements based upon the 'topmost' unique key given by the iterator then returning a dictionary of what was left over. 
I tried some other ways to do a lower level array slice but couldn't get the syntax to work in the time provided.

Program Instructions:

Run ConsoleCache

Type 'HELP' for the list of commands. Don't type 'OK'. OK I added a couple of extra verbs while making sure you get the expected output with the stated inputs. 
I was tempted to comment them out for correctness, but since my implementation's output is as expected I'm hoping you'll forgive me. I was envisioning this as a service a human might want to use also, 
so first I thought it'd be nice to have a HELP command. Further, I implemented these to demonstrate the ease of expansion over the initial command set. Also 'GETALL' is really handy for debugging.

Complexity:

I think the way the linq is written an eviction is going to be O(n), wanted O(1). I think this is still better than or equal to keeping a list and keeping track of the max
or keeping a sorted list of insertion/addition times by key, etc.
A get reorders the list and the way that is written it'll be O(n), was O(1) but the underlying data structure didn't cooperate with the ordering and is documented as such. See comments.
Without FIFO dictionary get by key is O(1). 

Better Solution:

Write a custom cache type like queue and dictionary which supports O(1) remove first/last, O(1) lookup by key, and O(1) move middle to last function... and 
where remove-first doesn't leave an empty gap at the start of the underlying data structure which will be the next one filled with an add.

Regards
Robi

