import pandas as pd
import json
import os

# Получаем путь к директории, где находится данный скрипт
script_dir = os.path.dirname(__file__)  # Путь к директории со скриптом

# Имя вашего JSON файла
json_file_name = 'output.json'  # Замените на имя вашего JSON файла
json_file_path = os.path.join(script_dir, json_file_name)

# Путь, по которому будет создан Excel файл
excel_file_path = os.path.join(script_dir, 'output.xlsx')  # Excel файл будет создан в той же директории

try:
    # Чтение JSON файла
    with open(json_file_path, 'r', encoding='utf-8') as json_file:
        data = json.load(json_file)

    # Проверка, что данные не пустые
    if not data:
        raise ValueError("JSON файл пуст.")

    # Создание DataFrame из данных JSON
    df = pd.DataFrame(data)

    # Сохранение DataFrame в Excel файл
    df.to_excel(excel_file_path, index=False, engine='openpyxl')

    print(f'Excel файл создан: {excel_file_path}')

except FileNotFoundError:
    print(f'Ошибка: Файл {json_file_path} не найден.')
except json.JSONDecodeError:
    print('Ошибка: Файл JSON некорректен.')
except ValueError as ve:
    print(ve)
except Exception as e:
    print(f'Произошла непредвиденная ошибка: {e}')