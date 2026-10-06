# Restaurant Ordering System

## Introduction

The project showcases how SOLID principles are applied in modern
software development. A Restaurant Ordering System was chosen as
the domain model to demonstrate these concepts.

## Defining the problem

The Restaurant has some finite number of tables. Each table has a
finite number of chairs. On some chairs it is allowed to sit to
anybody, while on others not, because they are ordered via our system.

## Defining Use Cases

So, the 'user' of restaurant can firstly to 'communicate' with this
restaurant: to come, to seat, to buy, to leave, etc. But they will
not be allowed to seat on the ordered chairs. So, we need to
design use cases for this ordering system, not for the restaurant
itself.

Firstly, before user can order something, he wants to see what is
already ordered. After that, we will tell us his credentials. He
will not be able to see who exactly ordered which place, but Admin
could do this.

So, use cases are:
User:

  1. View available places for ordering.
  2. Order available place. (by telling his name and phone number)
  3. Cancel the order.

Admin (everything that user can):

  1. Look who ordered which place.

## Small assumptions

As it is small project, the authentication system is not done,
it is out of scope. We just assume that Administrator has
his own password that he created before. For now the password
for admin is 'admin'.

## Why OOP?

So, we need to create ordering system of chairs. We might manage
them as a sequence of bits, where 0 is unordered and 1 is ordered.
The group of 5 bits will represent chairs near one table. This
could work good if the chairs would be welded together and the
tables to be built directly into the structural load-bearing walls.
Why only in that scenario? Imagine how the restaurant buys new table,
moves chairs so that now one table has more chairs than another. The
logic representation of bits even if still will work will require too
much cognitive loading for humans, and they will rather use notepad
instead of our application. That is why we need OOP, code need to be
consistent with real world.

## Design the classes

We are working with chairs and with tables. So, normally we can do:

```C#
public class Chair {
  private boolean isOrdered;
  private String orderedByName;
  private String orderedByPhone;
  
  public orrderChair(){

  }

  public cancelOrder(){

  }

}
```

In this implementation, Chair class has more than one responsibility.
By ordering, it needs also to get information from user what is his name
and phone number. And after a registration will be done, we could not by
one command change for example the name who ordered this chair, in case
when user will want to change nickname.
