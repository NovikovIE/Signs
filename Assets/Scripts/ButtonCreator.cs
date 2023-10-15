using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonCreator : MonoBehaviour
{
    [SerializeField]
    public enum Objects
    {
        PLUS,
        MINUS,
        NUMBER,
        MULTIPLY,
        DIVIDE,
        EQUALS,
        TILE,
        SIGN
    }

    [SerializeField] private int[,] numbers;
    [SerializeField] private Objects[,] object_types;

    public GameObject[] prefab_numbers;

    public GameObject prefab_plus;
    public GameObject prefab_minus;
    public GameObject prefab_multiply;
    public GameObject prefab_divide;
    public GameObject prefab_equals;
    public GameObject prefab_tile;

    [SerializeField] private GameObject ad_controller;


    [SerializeField] public int temp_field_size_type { set; get; } = 1;
    [SerializeField] public int field_size_type = 1;
    [SerializeField] public int size_of_field = 3;
    [SerializeField] public int prev_field_size = 3;
    private const double height = 10;
    private const double width = 4.4;
    private const int min_field_size = 2;
    private double position;

    private static readonly double[] button_scales = new double[] { 0.8, 0.59, 0.45, 0.39 };
    private static readonly double[] additional_scales = new double[] { 1, 0.89, 0.7, 0.44, 0.35 };

    public GameObject[,] prefabs;

    [SerializeField] private int right_answers;
    public int[] filled_tiles_row;
    public int[] filled_tiles_column;
    public bool[] right_answers_row;
    public bool[] right_answers_column;

    [SerializeField] public bool temp_is_correct_operation_order { set; get; } = true;

    [SerializeField] public bool is_correct_operation_order { set; get; } = true;
    [SerializeField] public bool is_signs_mode = true;

    [SerializeField] private GameObject timer;

    public void Start()
    {
        restart();
    }

    public void restart()
    {
        try { destroy_field(); }
        catch { }
        var tt = timer.GetComponent<Timer>();
        if (tt.time >= 120)
        {
            ad_controller.GetComponent<IniterstitialAds>().ShowAd();
            tt.time = 0;
        }
        right_answers = 0;
        size_of_field = field_size_type + 2;
        prev_field_size = size_of_field;
        initialize_field();
        debug_field();
        show_field();
    }

    public void settings_exit() {
        if (temp_is_correct_operation_order != is_correct_operation_order || temp_field_size_type != field_size_type) {
            is_correct_operation_order = temp_is_correct_operation_order;
            field_size_type = temp_field_size_type;
            restart();
        }
    }

    // create prefabs in game
    private void show_field()
    {
        Vector3[,] pos = new Vector3[object_types.GetLength(0), object_types.GetLength(0)];

        calculate_positions(ref pos);

        // create objects in game
        for (var i = 0; i < object_types.GetLength(0); ++i)
        {
            for (var j = 0; j < object_types.GetLength(1); ++j)
            {
                if (object_types[i, j] == Objects.TILE) continue;
                var scale_vector = new Vector3(((float)button_scales[size_of_field - min_field_size]), ((float)button_scales[size_of_field - min_field_size]), 1);
                if (is_signs_mode)
                {
                    if (object_types[i, j] == Objects.NUMBER) instantiate_number(numbers[i / 2, j / 2], pos[i, j], i, j);
                    else if (object_types[i, j] == Objects.EQUALS) instantiate_equals(pos[i, j], i, j);
                    else instantiate_tile(pos[i, j], i, j);
                }
                else
                {
                    if (object_types[i, j] == Objects.NUMBER)
                    {
                        if (i == object_types.GetLength(0) - 1 || j == object_types.GetLength(1) - 1) instantiate_number(numbers[i / 2, j / 2], pos[i, j], i, j);
                        else instantiate_tile(pos[i, j], i, j);
                    }
                    else if (object_types[i, j] == Objects.EQUALS) instantiate_equals(pos[i, j], i, j);
                    else instantiate_operation(object_types[i, j], pos[i, j], i, j);
                }
            }
        }
    }

    private void instantiate_equals(Vector3 pos, int i, int j)
    {
        if (i == object_types.GetLength(0) - 2) prefabs[i, j] = instantiate_prefab(prefab_equals, pos, new Vector3(0, 0, 90), 1);
        else prefabs[i, j] = instantiate_prefab(prefab_equals, pos, new Vector3(0, 0, 0), 1);
        var temp = prefabs[i, j].GetComponent<FieldFrames>();
        temp.i = i;
        temp.j = j;
    }

    private void instantiate_tile(Vector3 pos, int i, int j)
    {
        prefabs[i, j] = instantiate_prefab(prefab_tile, pos, new Vector3(0, 0, 0), 1);
        var temp_tile = prefabs[i, j].GetComponent<Tile>();
        temp_tile.i = i;
        temp_tile.j = j;
        var temp_ff = prefabs[i, j].GetComponent<FieldFrames>();
        temp_ff.i = i;
        temp_ff.j = j;
    }

    public GameObject instantiate_operation(Objects obj, Vector3 pos, int i, int j)
    {
        if (obj == Objects.PLUS) prefabs[i, j] = instantiate_prefab(prefab_plus, pos, new Vector3(0, 0, 0), 1);
        else if (obj == Objects.MINUS) prefabs[i, j] = instantiate_prefab(prefab_minus, pos, new Vector3(0, 0, 0), 1);
        else if (obj == Objects.MULTIPLY) prefabs[i, j] = instantiate_prefab(prefab_multiply, pos, new Vector3(0, 0, 0), 1);
        else if (obj == Objects.DIVIDE) prefabs[i, j] = instantiate_prefab(prefab_divide, pos, new Vector3(0, 0, 0), 1);
        var temp = prefabs[i, j].GetComponent<FieldFrames>();
        temp.i = i;
        temp.j = j;
        return prefabs[i, j];
    }

    public FieldFrames instantiate_number(int number, Vector3 pos, int i, int j)
    {
        if (number < 10 && number >= 0) prefabs[i, j] = instantiate_prefab(prefab_numbers[number], pos, new Vector3(0, 0, 0), 1);
        else instantiate_complicated_number(number, pos, i, j);
        var temp = prefabs[i, j].GetComponent<FieldFrames>();
        temp.i = i;
        temp.j = j;
        return temp;
    }

    // calculate positions
    private void calculate_positions(ref Vector3[,] pos)
    {
        for (var i = 0; i < object_types.GetLength(0); ++i)
            for (var j = 0; j < object_types.GetLength(1); ++j)
                pos[i, j] = new Vector3(((float)(position * j + position / 2 - width / 2)),
                    ((float)(-position * i - position / 2 + width / 2)), 0);
    }

    // instantiate prefab with needed position, rotation and scale
    public GameObject instantiate_prefab(GameObject pref, Vector3 pos, Vector3 rotation, float additional_scale)
    {
        GameObject temp = Instantiate(pref, pos, Quaternion.Euler(rotation));
        Vector3 previous_scale = temp.transform.localScale;
        temp.transform.localScale = new Vector3(
            previous_scale.x * ((float)button_scales[size_of_field - min_field_size] * additional_scale),
            previous_scale.y * ((float)button_scales[size_of_field - min_field_size] * additional_scale),
            previous_scale.z
        );
        return temp;
    }

    // Instantiate number which is < 0 or > 9
    private void instantiate_complicated_number(int number, Vector3 pos, int row, int column)
    {
        var object_length = 0;
        if (number < 0)
        {
            ++object_length;
            number = -number;
        }

        List<int> digits = new List<int>();
        while (number != 0)
        {
            digits.Add(number % 10);
            number /= 10;
            ++object_length;
        }

        digits.Reverse();
        double step = position / object_length;
        double current_pos = pos.x - position / 2 + step / 2;
        GameObject prev = null;
        if (object_length != digits.Count)
        {
            prev = instantiate_prefab(prefab_minus, new Vector3(((float)current_pos), pos.y, pos.z), new Vector3(0, 0, 0), ((float)additional_scales[object_length - 1]));
            current_pos += step;
            prefabs[row, column] = prev;
        }

        for (int i = 0; i < digits.Count; ++i)
        {
            GameObject temp = instantiate_prefab(prefab_numbers[digits[i]], new Vector3(((float)current_pos), pos.y, pos.z), new Vector3(0, 0, 0), ((float)additional_scales[object_length - 1]));
            if (prev != null) prev.GetComponent<FieldFrames>().tile_under = temp;
            else prefabs[row, column] = temp;
            prev = temp;
            var ff = temp.GetComponent<FieldFrames>();
            ff.i = row;
            ff.j = column;
            current_pos += step;
        }
    }

    void define_object_types(int inner_field_size)
    {
        for (int i = 0; i < inner_field_size; ++i)
            for (int j = 0; j < inner_field_size; ++j)
            {
                if ((i % 2 == 1 && j % 2 == 1) || (i >= inner_field_size - 2 && j >= inner_field_size - 2) || (i == inner_field_size - 1 && j % 2 == 1) || (j == inner_field_size - 1 && i % 2 == 1))
                    object_types[i, j] = Objects.TILE;
                else if (i % 2 == 0 && j % 2 == 0) object_types[i, j] = Objects.NUMBER;
                else if ((i == inner_field_size - 2 && j % 2 == 0) || (i % 2 == 0 && j == inner_field_size - 2)) object_types[i, j] = Objects.EQUALS;
                else if (i % 2 == 0 || j % 2 == 0) object_types[i, j] = Objects.SIGN;
                else object_types[i, j] = Objects.TILE;
            }
    }

    // create numbers
    void create_numbers(int inner_field_size)
    {
        for (int i = 0; i < inner_field_size - 2; i += 2)
            for (int j = 0; j < inner_field_size - 2; j += 2)
                if (object_types[i, j] == Objects.NUMBER)
                {
                    int value = Random.Range(0, 10);
                    numbers[i / 2, j / 2] = value;
                }
    }

    // create signs (rows)
    void create_signs_rows(int inner_field_size)
    {
        if (is_correct_operation_order)
            for (int i = 0; i < inner_field_size - 2; i += 2)
            {
                int temp_value = numbers[i / 2, 0];
                for (int j = 1; j < inner_field_size - 2; j += 2)
                {
                    int value;
                    if ((numbers[i / 2, (j + 1) / 2] != 0) && (temp_value % numbers[i / 2, (j + 1) / 2] == 0))
                    {
                        value = Random.Range(0, 4);
                        if (value == 3) temp_value /= numbers[i / 2, (j + 1) / 2];
                        else if (value == 2) temp_value *= numbers[i / 2, (j + 1) / 2];
                        else temp_value = numbers[i / 2, (j + 1) / 2];
                    }
                    else
                    {
                        value = Random.Range(0, 3);
                        if (value == 2) temp_value *= numbers[i / 2, (j + 1) / 2];
                        else temp_value = numbers[i / 2, (j + 1) / 2];
                    }
                    fill_number(value, i, j);
                }
            }
        else
            for (int i = 0; i < inner_field_size - 2; i += 2)
                for (int j = 1; j < inner_field_size - 2; j += 2)
                {
                    int value;
                    if ((numbers[i / 2, (j + 1) / 2] != 0) && (numbers[i / 2, (j - 1) / 2] % numbers[i / 2, (j + 1) / 2] == 0)) value = Random.Range(0, 4);
                    else value = Random.Range(0, 3);
                    fill_number(value, i, j);
                }
    }

    // create signs (columns)
    void create_signs_columns(int inner_field_size)
    {
        if (is_correct_operation_order)
            for (int j = 0; j < inner_field_size - 2; j += 2)
            {
                int temp_value = numbers[0, j / 2];
                for (int i = 1; i < inner_field_size - 2; i += 2)
                {
                    int value;
                    if ((numbers[(i + 1) / 2, j / 2] != 0) && (temp_value % numbers[(i + 1) / 2, j / 2] == 0))
                    {
                        value = Random.Range(0, 4);
                        if (value == 3) temp_value /= numbers[(i + 1) / 2, j / 2];
                        else if (value == 2) temp_value *= numbers[(i + 1) / 2, j / 2];
                        else temp_value = numbers[(i + 1) / 2, j / 2];
                    }
                    else
                    {
                        value = Random.Range(0, 3);
                        if (value == 2) temp_value *= numbers[(i + 1) / 2, j / 2];
                        else temp_value = numbers[(i + 1) / 2, j / 2];
                    }
                    fill_number(value, i, j);
                }
            }
        else
            for (int j = 0; j < inner_field_size - 2; j += 2)
                for (int i = 1; i < inner_field_size - 2; i += 2)
                {
                    int value;
                    if ((numbers[(i + 1) / 2, j / 2] != 0) && (numbers[(i - 1) / 2, j / 2] % numbers[(i + 1) / 2, j / 2] == 0)) value = Random.Range(0, 4);
                    else value = Random.Range(0, 3);
                    fill_number(value, i, j);
                }
    }

    // initialize field
    public void initialize_field()
    {
        filled_tiles_row = new int[size_of_field];
        filled_tiles_column = new int[size_of_field];
        right_answers_row = new bool[size_of_field];
        right_answers_column = new bool[size_of_field];
        prefabs = new GameObject[2 * size_of_field + 1, 2 * size_of_field + 1];
        position = width / (size_of_field * 2 + 1);
        int inner_field_size = size_of_field * 2 + 1;
        object_types = new Objects[inner_field_size, inner_field_size];
        numbers = new int[size_of_field + 1, size_of_field + 1];

        for (var i = 0; i < size_of_field; ++i)
        {
            right_answers_row[i] = false;
            right_answers_column[i] = false;
        }

        fill_array_with_zero_1(ref filled_tiles_row);
        fill_array_with_zero_1(ref filled_tiles_column);
        fill_array_with_zero_2(ref numbers);

        define_object_types(inner_field_size);
        create_numbers(inner_field_size);
        create_signs_rows(inner_field_size);
        create_signs_columns(inner_field_size);

        debug_show_numbers();
        if (is_correct_operation_order) calculate_equations_with_order(inner_field_size);
        else calculate_equations_without_order(inner_field_size);
        debug_show_numbers();
    }

    private void fill_array_with_zero_1(ref int[] array)
    {
        for (int i = 0; i < array.GetLength(0); ++i)
            array[i] = 0;
    }

    private void fill_array_with_zero_2(ref int[,] array)
    {
        for (int i = 0; i < array.GetLength(0); ++i)
            for (int j = 0; j < array.GetLength(1); ++j)
                array[i, j] = 0;
    }

    // create sign depends on value
    private void fill_number(int value, int i, int j)
    {
        if (value == 0) object_types[i, j] = Objects.PLUS;
        else if (value == 1) object_types[i, j] = Objects.MINUS;
        else if (value == 2) object_types[i, j] = Objects.MULTIPLY;
        else if (value == 3) object_types[i, j] = Objects.DIVIDE;
    }

    private void calculate_operation(ref int value, int number, Objects type)
    {
        if (type == Objects.PLUS) value += number;
        else if (type == Objects.MINUS) value -= number;
        else if (type == Objects.MULTIPLY) value *= number;
        else if (type == Objects.DIVIDE) value /= number;
    }

    // calculate operation between two close numbers in a row
    private void calculate_operation_horizontal(ref int value, int i, int j, Objects type)
    {
        calculate_operation(ref value, numbers[i / 2, (j + 1) / 2], type);
    }

    // calculate operation between two close numbers in a column
    private void calculate_operation_vertical(ref int value, int i, int j, Objects type)
    {
        calculate_operation(ref value, numbers[(i + 1) / 2, j / 2], type);
    }

    private void calculate_row(int row)
    {
        var value = numbers[row / 2, 0];
        for (var j = 1; j < object_types.GetLength(0) - 2; j += 2)
            calculate_operation_horizontal(ref value, row, j, object_types[row, j]);
        numbers[row / 2, (object_types.GetLength(0) - 1) / 2] = value;
    }

    private void calculate_column(int column)
    {
        int value = numbers[0, column / 2];
        for (var i = 1; i < object_types.GetLength(1) - 2; i += 2)
            calculate_operation_vertical(ref value, i, column, object_types[i, column]);
        numbers[(object_types.GetLength(1) - 1) / 2, column / 2] = value;
    }

    // calculate the numbers after "="
    private void calculate_equations_without_order(int inner_field_size)
    {
        // create equations (rows)
        for (var i = 0; i < inner_field_size - 2; i += 2)
            calculate_row(i);

        // create equations (columns)
        for (var j = 0; j < inner_field_size - 2; j += 2)
            calculate_column(j);
    }

    private void calculate_row_with_order(int row)
    {
        List<Objects> operations = new List<Objects>();
        List<int> calculated_numbers = new List<int>();
        var value = numbers[row / 2, 0];
        for (var j = 1; j < object_types.GetLength(0) - 2; j += 2)
            if (object_types[row, j] == Objects.MULTIPLY || object_types[row, j] == Objects.DIVIDE)
            {
                calculate_operation_horizontal(ref value, row, j, object_types[row, j]);
            }
            else
            {
                calculated_numbers.Add(value);
                operations.Add(object_types[row, j]);
                value = numbers[row / 2, (j + 1) / 2];
            }
        calculated_numbers.Add(value);
        value = calculated_numbers[0];
        for (var j = 0; j < operations.Count; ++j)
            calculate_operation(ref value, calculated_numbers[j + 1], operations[j]);
        numbers[row / 2, (object_types.GetLength(0) - 1) / 2] = value;
    }

    private void calculate_column_with_order(int column)
    {
        List<Objects> operations = new List<Objects>();
        List<int> calculated_numbers = new List<int>();
        var value = numbers[0, column / 2];
        for (var j = 1; j < object_types.GetLength(1) - 2; j += 2)
            if (object_types[j, column] == Objects.MULTIPLY || object_types[j, column] == Objects.DIVIDE)
            {
                calculate_operation_vertical(ref value, j, column, object_types[j, column]);
            }
            else
            {
                calculated_numbers.Add(value);
                operations.Add(object_types[j, column]);
                value = numbers[(j + 1) / 2, column / 2];
            }
        calculated_numbers.Add(value);
        value = calculated_numbers[0];
        for (var j = 0; j < operations.Count; ++j)
            calculate_operation(ref value, calculated_numbers[j + 1], operations[j]);
        numbers[(object_types.GetLength(0) - 1) / 2, column / 2] = value;
    }

    private void calculate_equations_with_order(int inner_field_size)
    {
        // create equations (rows)
        for (var i = 0; i < inner_field_size - 2; i += 2)
            calculate_row_with_order(i);

        // create equations (columns)
        for (var j = 0; j < inner_field_size - 2; j += 2)
            calculate_column_with_order(j);
    }

    private void check_equation_correctness_row_without_order(int row)
    {
        int value;
        if (is_signs_mode)
        {
            value = numbers[row / 2, 0];
            for (var j = 1; j < object_types.GetLength(0) - 2; j += 2)
            {
                if (prefabs[row, j].GetComponent<FieldFrames>().type == Objects.DIVIDE &&
                   (numbers[row / 2, (j + 1) / 2] == 0 || (numbers[row / 2, (j - 1) / 2] % numbers[row / 2, (j + 1) / 2] != 0)))
                {
                    switch_row_color(row, Colors.RED);
                    return;
                }
                calculate_operation_horizontal(ref value, row, j, prefabs[row, j].GetComponent<FieldFrames>().type);
            }
        }
        else
        {
            value = prefabs[row, 0].GetComponent<FieldFrames>().value;
            for (var j = 1; j < object_types.GetLength(0) - 2; j += 2)
            {
                int number = prefabs[row, j + 1].GetComponent<FieldFrames>().value;
                if (object_types[row, j] == Objects.DIVIDE && (number == 0 || (value % number != 0)))
                {
                    switch_row_color(row, Colors.RED);
                    return;
                }
                calculate_operation(ref value, number, object_types[row, j]);
            }
        }
        if (value == numbers[row / 2, (object_types.GetLength(0) - 1) / 2])
        {
            ++right_answers;
            right_answers_row[row / 2] = true;
            switch_row_color(row, Colors.GREEN);
        }
        else switch_row_color(row, Colors.RED);
    }

    private void check_equation_correctness_row_with_order(int row)
    {
        List<Objects> operations = new List<Objects>();
        List<int> calculated_numbers = new List<int>();
        int value;
        if (is_signs_mode)
        {
            value = numbers[row / 2, 0];
            for (var j = 1; j < prefabs.GetLength(0) - 2; j += 2)
            {
                Objects type = prefabs[row, j].GetComponent<FieldFrames>().type;
                if (type == Objects.DIVIDE && (numbers[row / 2, (j + 1) / 2] == 0 || (value % numbers[row / 2, (j + 1) / 2] != 0)))
                {
                    switch_row_color(row, Colors.RED);
                    return;
                }
                if (type == Objects.MULTIPLY || type == Objects.DIVIDE)
                {
                    calculate_operation_horizontal(ref value, row, j, type);
                }
                else
                {
                    calculated_numbers.Add(value);
                    operations.Add(type);
                    value = numbers[row / 2, (j + 1) / 2];
                }
            }
        }
        else
        {
            value = prefabs[row, 0].GetComponent<FieldFrames>().value;
            for (var j = 1; j < prefabs.GetLength(0) - 2; j += 2)
            {
                int number = prefabs[row, j + 1].GetComponent<FieldFrames>().value;
                if (object_types[row, j] == Objects.DIVIDE && (number == 0 || (value % number != 0)))
                {
                    switch_row_color(row, Colors.RED);
                    return;
                }
                if (object_types[row, j] == Objects.MULTIPLY || object_types[row, j] == Objects.DIVIDE)
                {
                    calculate_operation(ref value, number, object_types[row, j]);
                }
                else
                {
                    calculated_numbers.Add(value);
                    operations.Add(object_types[row, j]);
                    value = number;
                }
            }
        }
        calculated_numbers.Add(value);
        value = calculated_numbers[0];
        for (var j = 0; j < operations.Count; ++j)
            calculate_operation(ref value, calculated_numbers[j + 1], operations[j]);
        if (value == numbers[row / 2, (object_types.GetLength(0) - 1) / 2])
        {
            ++right_answers;
            right_answers_row[row / 2] = true;
            switch_row_color(row, Colors.GREEN);
        }
        else switch_row_color(row, Colors.RED);
    }

    private void check_equation_correctness_row(int row)
    {
        if (is_signs_mode && filled_tiles_row[row / 2] != size_of_field - 1) return;
        if (!is_signs_mode && filled_tiles_row[row / 2] != size_of_field) return;
        if (is_correct_operation_order) check_equation_correctness_row_with_order(row);
        else check_equation_correctness_row_without_order(row);
    }

    private void check_equation_correctness_column_without_order(int column)
    {
        int value;
        if (is_signs_mode)
        {
            value = numbers[0, column / 2];
            for (var i = 1; i < object_types.GetLength(1) - 2; i += 2)
            {
                if (prefabs[i, column].GetComponent<FieldFrames>().type == Objects.DIVIDE &&
                   (numbers[(i + 1) / 2, column / 2] == 0 || (value % numbers[(i + 1) / 2, column / 2] != 0)))
                {
                    switch_column_color(column, Colors.RED);
                    return;
                }
                calculate_operation_vertical(ref value, i, column, prefabs[i, column].GetComponent<FieldFrames>().type);
            }
        }
        else
        {
            value = prefabs[0, column].GetComponent<FieldFrames>().value;
            for (var i = 1; i < object_types.GetLength(1) - 2; i += 2)
            {
                int number = prefabs[i + 1, column].GetComponent<FieldFrames>().value;
                if (object_types[i, column] == Objects.DIVIDE && (number == 0 || (value % number != 0)))
                {
                    switch_column_color(column, Colors.RED);
                    return;
                }
                calculate_operation(ref value, number, object_types[i, column]);
            }
        }
        if (value == numbers[(prefabs.GetLength(1) - 1) / 2, column / 2])
        {
            ++right_answers;
            right_answers_column[column / 2] = true;
            switch_column_color(column, Colors.GREEN);
        }
        else switch_column_color(column, Colors.RED);
    }

    private void check_equation_correctness_column_with_order(int column)
    {
        List<Objects> operations = new List<Objects>();
        List<int> calculated_numbers = new List<int>();
        int value;
        if (is_signs_mode)
        {
            value = numbers[0, column / 2];
            for (var i = 1; i < object_types.GetLength(1) - 2; i += 2)
            {
                Objects type = prefabs[i, column].GetComponent<FieldFrames>().type;
                if (type == Objects.DIVIDE && (numbers[(i + 1) / 2, column / 2] == 0 || (value % numbers[(i + 1) / 2, column / 2] != 0)))
                {
                    switch_column_color(column, Colors.RED);
                    return;
                }
                if (type == Objects.MULTIPLY || type == Objects.DIVIDE)
                {
                    calculate_operation_vertical(ref value, i, column, type);
                }
                else
                {
                    calculated_numbers.Add(value);
                    operations.Add(type);
                    value = numbers[(i + 1) / 2, column / 2];
                }
            }
        }
        else
        {
            value = prefabs[0, column].GetComponent<FieldFrames>().value;
            for (var i = 1; i < object_types.GetLength(1) - 2; i += 2)
            {
                int number = prefabs[i + 1, column].GetComponent<FieldFrames>().value;
                if (object_types[i, column] == Objects.DIVIDE && (number == 0 || (value % number != 0)))
                {
                    switch_column_color(column, Colors.RED);
                    return;
                }
                if (object_types[i, column] == Objects.MULTIPLY || object_types[i, column] == Objects.DIVIDE)
                {
                    calculate_operation(ref value, number, object_types[i, column]);
                }
                else
                {
                    calculated_numbers.Add(value);
                    operations.Add(object_types[i, column]);
                    value = number;
                }
            }
        }
        calculated_numbers.Add(value);
        value = calculated_numbers[0];
        for (var j = 0; j < operations.Count; ++j)
            calculate_operation(ref value, calculated_numbers[j + 1], operations[j]);
        if (value == numbers[(prefabs.GetLength(1) - 1) / 2, column / 2])
        {
            ++right_answers;
            right_answers_column[column / 2] = true;
            switch_column_color(column, Colors.GREEN);
        }
        else switch_column_color(column, Colors.RED);
    }

    private void check_equation_correctness_column(int column)
    {
        if (is_signs_mode && filled_tiles_column[column / 2] != size_of_field - 1) return;
        if (!is_signs_mode && filled_tiles_column[column / 2] != size_of_field) return;
        if (is_correct_operation_order) check_equation_correctness_column_with_order(column);
        else check_equation_correctness_column_without_order(column);
    }

    public void add_one_to_filled_tiles(int i, int j)
    {
        if (i % 2 == 0)
        {
            ++filled_tiles_row[i / 2];
            check_equation_correctness_row(i);
            check_win();
        }
        if (j % 2 == 0)
        {
            ++filled_tiles_column[j / 2];
            check_equation_correctness_column(j);
            check_win();
        }
    }

    public void substract_one_from_filled_tiles(int i, int j)
    {
        if (i % 2 == 0)
        {
            if (right_answers_row[i / 2])
            {
                --right_answers;
                right_answers_row[i / 2] = false;
            }
            --filled_tiles_row[i / 2];
            switch_row_color(i, Colors.BLUE);
        }
        if (j % 2 == 0)
        {
            if (right_answers_column[j / 2])
            {
                --right_answers;
                right_answers_column[j / 2] = false;
            }
            --filled_tiles_column[j / 2];
            switch_column_color(j, Colors.BLUE);
        }
    }

    enum Colors
    {
        BLUE, GREEN, RED
    }

    private void switch_row_color(int row, Colors color)
    {
        if (is_signs_mode)
        {
            if (color == Colors.RED)
                for (var j = 1; j < prefabs.GetLength(0); j += 2)
                    prefabs[row, j].GetComponent<FieldFrames>().switch_to_red();
            else if (color == Colors.BLUE)
                for (var j = 1; j < prefabs.GetLength(0); j += 2)
                    prefabs[row, j].GetComponent<FieldFrames>().switch_to_blue();
            else
                for (var j = 1; j < prefabs.GetLength(0); j += 2)
                    prefabs[row, j].GetComponent<FieldFrames>().switch_to_green();
        }
        else
        {
            if (color == Colors.RED)
                for (var j = 0; j < prefabs.GetLength(0) - 1; j += 2)
                {
                    if (right_answers_column[j / 2]) prefabs[row, j].GetComponent<FieldFrames>().switch_to_green_red();
                    else prefabs[row, j].GetComponent<FieldFrames>().switch_to_red();
                }
            else if (color == Colors.BLUE)
                for (var j = 0; j < prefabs.GetLength(0) - 1; j += 2)
                {
                    if (!right_answers_column[j / 2] && filled_tiles_column[j / 2] != size_of_field) prefabs[row, j].GetComponent<FieldFrames>().switch_to_blue();
                    else if (!right_answers_column[j / 2] && filled_tiles_column[j / 2] == size_of_field) prefabs[row, j].GetComponent<FieldFrames>().switch_to_red();
                    else prefabs[row, j].GetComponent<FieldFrames>().switch_to_green();
                }
            else
                for (var j = 0; j < prefabs.GetLength(0) - 1; j += 2)
                {
                    if (!right_answers_column[j / 2] && filled_tiles_column[j / 2] == size_of_field) prefabs[row, j].GetComponent<FieldFrames>().switch_to_green_red();
                    else prefabs[row, j].GetComponent<FieldFrames>().switch_to_green();
                }
        }
    }

    private void switch_column_color(int column, Colors color)
    {
        if (is_signs_mode)
        {
            if (color == Colors.RED)
                for (var i = 1; i < prefabs.GetLength(1); i += 2)
                    prefabs[i, column].GetComponent<FieldFrames>().switch_to_red();
            else if (color == Colors.BLUE)
                for (var i = 1; i < prefabs.GetLength(1); i += 2)
                    prefabs[i, column].GetComponent<FieldFrames>().switch_to_blue();
            else
                for (var i = 1; i < prefabs.GetLength(1); i += 2)
                    prefabs[i, column].GetComponent<FieldFrames>().switch_to_green();
        }
        else
        {
            if (color == Colors.RED)
                for (var i = 0; i < prefabs.GetLength(1) - 1; i += 2)
                {
                    if (right_answers_row[i / 2]) prefabs[i, column].GetComponent<FieldFrames>().switch_to_green_red();
                    else prefabs[i, column].GetComponent<FieldFrames>().switch_to_red();
                }
            else if (color == Colors.BLUE)
                for (var i = 0; i < prefabs.GetLength(1) - 1; i += 2)
                {
                    if (!right_answers_row[i / 2] && filled_tiles_row[i / 2] != size_of_field) prefabs[i, column].GetComponent<FieldFrames>().switch_to_blue();
                    else if (!right_answers_row[i / 2] && filled_tiles_row[i / 2] == size_of_field) prefabs[i, column].GetComponent<FieldFrames>().switch_to_red();
                    else prefabs[i, column].GetComponent<FieldFrames>().switch_to_green();
                }
            else
                for (var i = 0; i < prefabs.GetLength(1) - 1; i += 2)
                {
                    if (!right_answers_row[i / 2] && filled_tiles_row[i / 2] == size_of_field) prefabs[i, column].GetComponent<FieldFrames>().switch_to_green_red();
                    else prefabs[i, column].GetComponent<FieldFrames>().switch_to_green();
                }
        }
    }

    public void destroy_field()
    {
        for (int i = 0; i < prev_field_size * 2 + 1; ++i)
            for (int j = 0; j < prev_field_size * 2 + 1; ++j)
                while (prefabs[i, j] != null)
                {
                    prefabs[i, j].GetComponent<FieldFrames>().is_game_resets = true;
                    DestroyImmediate(prefabs[i, j]);
                }
    }
    
    public void check_win()
    {
        if (right_answers == (size_of_field * 2))
        {
            restart();
        }
    }

    [SerializeField]
    public void set_field_size_type(int new_size)
    {
        field_size_type = new_size;
    }

    ///////////////////////////////////////////////////////////////////////////
    ///                                DEBUG                                ///
    ///////////////////////////////////////////////////////////////////////////

    private void debug_show_numbers()
    {
        for (var i = 0; i < size_of_field + 1; ++i)
        {
            string outline = i + " row: ";

            for (var j = 0; j < size_of_field + 1; ++j)
                outline += numbers[i, j].ToString() + " ";

            UnityEngine.Debug.Log(outline);
        }
    }

    private void debug_field()
    {
        for (var i = 0; i < size_of_field * 2 + 1; ++i)
        {
            string outline = i + " row: ";
            for (var j = 0; j < size_of_field * 2 + 1; ++j)
            {
                string sign = " ";
                if (object_types[i, j] == Objects.PLUS) sign = "+";
                else if (object_types[i, j] == Objects.MINUS) sign = "-";
                else if (object_types[i, j] == Objects.MULTIPLY) sign = "*";
                else if (object_types[i, j] == Objects.DIVIDE) sign = "/";
                else if (object_types[i, j] == Objects.EQUALS) sign = "=";
                else if (object_types[i, j] == Objects.NUMBER) sign = numbers[i / 2, j / 2].ToString();
                outline += sign;
            }
            UnityEngine.Debug.Log(outline);
        }
    }
}