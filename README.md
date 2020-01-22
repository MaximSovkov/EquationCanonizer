# EquationCanonizer
## Задание

Напишите программу на C#, которая преобразует уравнение к каноническому виду: приводит подобные слагаемые, сортирует, приравнивает сумму к нулю и т.д.
Уравнение может быть любого порядка, может содержать любое количество переменных, может быть записано со скобками (в этом случае приложение должно раскрыть скобки).
Например, может быть дано уравнение следующего вида:

    x^2 + 3.5xy + y = -4 + y^2 - yx + y

Оно должно быть приведено к виду:

    x^2 + 4.5xy - y^2 + 4 = 0

Программа должна быть оформлена как консольное приложение Visual Studio и работать в интерактивном режиме.
Если входные данные некорректны (например, "x=y=0=^2"), то должно быть выведено сообщение о соответствующей ошибке.
Должны быть написаны некоторые юнит-тесты через NUnit.
Постановка задачи некорректная. Решите её настолько обобщённо, насколько посчитаете возможным.
Ориентировочное время написания решения – 5 часов. Решение должно быть выслано не позже, чем через неделю с момента получения задачи.

## Описание решения

У меня было 2 идеи, с помощью которых можно было решить задание: разбить выражение на токены и парсить вручную или же воспользоваться Antlr и решить некоторую часть проблем с помощью кодогенерации.
Однако, я решил не использовать Antlr, поскольку, наверное, это не совсем "честно" в данном задании. Также, я достаточно плохо с ним знаком, поэтому мне потребовалось бы время (в котором я ограничен по условиям задачи) для изучения этой библиотеки.

## Минусы решения

 * Термы `xx` и `x^2` воспринимаются как разные.
 * Отсутствует поддержка умножения (`*`) и деления (`/`). Также, не поддерживается коэффициент перед скобками (`3(x + y)`).
 * Посредственная валидация данных. Конечно есть некоторые сообщения об ошибках, но на самом деле, этот пункт можно очень долго улучшать, так как количество способ ввести невалидные данные очень большое.
 * Не совсем корректная обработка исключений. Я выбрасываю исключения одного типа (возможно, стоило написать своё собственное исключение) и ловлю их в глобальном обработчике.
