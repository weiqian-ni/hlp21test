module Answers

(*
//-------------------------TBL Class Prep Test 1-----------------------------//
This file contains questions as comments and should be edited so that you 
replace the failwithf expressions in the definitions of q1-q6 by your answers.

The file can be run in the practicetest1 solution to check the program 
answers (3-6) with a testbench. The Multiple choice questions (1-2) will be 
checked to see if you have attempted them and if so the answer checked if  
it is one of the allowed values.

MCQ questions are not marked by this testbench,
they will be marked in the submitted code.

When you have completed the test please submit your answers.fs file as specified.

This style of test will be used for the two TBL tests 
and also for the assessed mid-term test.
//---------------------------------------------------------------------------//
*)

(* 
Q1. How many values does the F# Unit type have?
0 or 1 are allowed answers
(Q1 is a function that must return the answer)
*)
let q1() : int = failwithf "Not answered"


(*
Q2. The F# type constructor -> has what associativity?
0 = left associative
1 = right associative
2 = associativity does not apply to type constructors
(Q2 is a function that must return the correct answer)
*)
let q2() : int = failwithf "Not answered"



(*
Q3. The output list is twice the length of the input list. Each input list element occurs in order
in the output, twice. E.g [1;2;5] -> [1;1;2;2;5;5].
You are not allowed to use list indexing (.[] or List.item) in your answer.
*)
let q3 (lst: int list) : int list = failwithf "Not answered"


(*
Q4. The output is the sum of all the elements in all the input lists.
Recursive functions are not allowed in the answer.
*)
let q4 (lsts: int list list) : int = failwithf "Not answered"



(*
Q5. The output is the mode (element with maximum number of occurences) in the input list.
Thus [1;2;0;3;2;1;6;6;1] -> 1
If there is more than modal element the output should be the most positive of all such.
You may assume there is at least one element in the list.
HINT: consider List.countBy for one solution (there are others)
*)
let q5 (lst: int list): int = failwithf "Not answered"


(* 
Q6. List elements are numbered from 0.
Element n in the output list is the product of elements 2n and 2n+1 in the input list
If the input list has an odd number of elements then the last element of the output list
is the square of the last element of the input list.
You are not allowed to use list indexing (.[] or List.item) in your answer.
HINT: consider List.chunkBySize for one solution (there are others)
*)
let q6 (lst: int list): int list = failwithf "Not answered"

