# Tables

### 2x2

A1 | A2
-- | --
B1 | B2


### 3x3

A1|A2|A3
-|-|-
B1|B2|B3
C1|C2|C3


### Cell Alignment

Column | Column | Column
:----- | :----: | -----:
Left   | Center | Right
align  | align  | align

### Empty Cells

| | Digit | Character
| ------ | ------|----
| a      | 4     | $
| | 365   | (
| b      |       | ^  

### Escape Character

|  |
|-|
| This is a single cell containing a \| character |


<!-- not a standard markdown feature ...

### Merged Cells

|             |          Grouping           ||
First Header  | Second Header | Third Header |
 ------------ | :-----------: | -----------: |
Content       |          *Long Cell*        ||
Content       |   **Cell**    |         Cell |

New section   |     More      |         Data |
And more      | With an escaped '\|'         || 
-->

### Ignore Extra Cells

| Header 1  | Header 2            | Header 3  |
| --------- | ------------------- | --------- |
| Some data | Cell 2              | Cell 3    | Ignored | Ignored |
| data      | Some long data here | more data | 

### With Some Content

A1|![](https://via.placeholder.com/50x50)|A3
-|-|-
default|`code`|**bold** and *italic*
| | image ![](https://via.placeholder.com/50x50)|C3

