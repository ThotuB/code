nums = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10]
objs = [
    { 'name': 'John'    , 'age': 20 },
    { 'name': 'Jane'    , 'age': 21 },
    { 'name': 'Joe'     , 'age': 22 },
    { 'name': 'Jack'    , 'age': 23 },
    { 'name': 'Jill'    , 'age': 24 },
    { 'name': 'Jim'     , 'age': 25 },
    { 'name': 'Jenny'   , 'age': 26 },
]

def numsSort(num):
    return num % 2

def objsSort(obj):
    return len(obj['name'])

nums.sort(key=numsSort)
objs.sort(key=objsSort)

print(nums)
print(objs)