import json

from PyQt5.QtGui import QIntValidator
from PyQt5.QtWidgets import QVBoxLayout, QWidget, QTextEdit, \
    QPushButton, QHBoxLayout, QFileDialog, QGroupBox, QFormLayout, \
    QRadioButton, QLineEdit, QLabel, QMessageBox


class Menu(QWidget):
    def __init__(self):
        super().__init__()
        self.btn1 = QPushButton('Create new...')
        self.btn2 = QPushButton('Open...')

        self.hbox = QHBoxLayout()
        self.hbox.addWidget(self.btn1)
        self.hbox.addWidget(self.btn2)

        self.setLayout(self.hbox)


class Layout(QWidget):
    def __init__(self, layout):
        super().__init__()
        self.layout = layout
        self.setLayout(self.layout)

    def addWidget(self, widget):
        self.layout.addWidget(widget)


class MainPlot(QWidget):
    def __init__(self):
        super().__init__()
        self.loc = ''
        self.image_name = ''
        self.text = QTextEdit()
        self.right = Layout(QFormLayout())
        self.button = QPushButton('Image')

        self.group = QGroupBox()
        self.v = QVBoxLayout()
        self.locations = {
                'Underdark': QRadioButton('Андердарк'),
                'Larswood': QRadioButton('Лес Пледвэйл'),
                'TowerOfDurlag': QRadioButton('Башня Дурлага'),
                'FirewineRuins': QRadioButton('Руины Фаэрвайна')
        }
        for i in self.locations.values():
            self.v.addWidget(i)
        self.group.setLayout(self.v)
        self.button.clicked.connect(self.open_file_name_dialog)

        self.right.addWidget(self.button)
        self.right.addWidget(self.group)
        self.hbox = QHBoxLayout()
        self.hbox.addWidget(self.text)
        self.hbox.addWidget(self.right)

        self.setLayout(self.hbox)

    def open_file_name_dialog(self):
        options = QFileDialog.DontUseNativeDialog
        file_name, _ = QFileDialog.getOpenFileName(self,
                                                   "Choose image",
                                                   "./Background",
                                                   "Images (*.png)",
                                                   options=options)
        if file_name:
            self.image_name = file_name[-file_name[::-1].find('/'):]

    def to_json(self, file):
        result = {'Text': str(self.text.toPlainText()),
                  'Image': self.image_name}
        folder = ''
        for k, v in self.locations.items():
            if v.isChecked():
                folder = k
        self.loc = folder
        if folder and self.image_name and result['Text']:
            file = f'./Plot/Cards/{file}.json'
            with open(file, 'w', encoding='utf-8') as f:
                json.dump(result, f, ensure_ascii=False)

    def clear(self):
        self.image_name = ''
        self.loc = ''
        self.text.setText('')
        for i in self.locations.values():
            if i.isChecked():
                i.setChecked(False)

    def get_location(self):
        for k, v in self.locations.items():
            if v.isChecked():
                return k
        return None


class ZeroOption(QWidget):
    names = [
            'Torch',
            'Herb',
            'Sword',
            'Gold',
            'Supplies',
            'Hp'
    ]

    def __init__(self):
        super().__init__()
        self.vbox = QVBoxLayout()
        self.box = QGroupBox()
        self.htop = QHBoxLayout()

        self.options = {k: QLineEdit('0') for k in self.names}
        for v in self.options.values():
            v.setValidator(QIntValidator(-100, 100))
        for k, v in self.options.items():
            self.htop.addWidget(QLabel(k))
            self.htop.addWidget(v)
        self.box.setLayout(self.htop)
        self.vbox.addWidget(self.box)
        self.setLayout(self.vbox)

    def to_json(self, file):
        result = {k: v.text() for k, v in self.options.items()}
        with open(f'./Plot/Options/{file}0.json', 'w',
                  encoding='utf-8') as f:
            json.dump(result, f, ensure_ascii=False)

    def clear(self):
        for widget in self.options.values():
            widget.setText('0')

    def init(self, dictionary):
        for k, v in dictionary.items():
            self.options[k].setText(v)


class Option(QWidget):
    options = [
            'Torch',
            'Herb',
            'Sword',
            'Gold',
            'Supplies',
            'Hp'
    ]

    def __init__(self):
        super().__init__()
        self.h = QHBoxLayout()
        self.box = QGroupBox()
        self.vbox = QVBoxLayout()
        self.requirements = {k: QLineEdit('0') for k in self.options[:-1]}
        for v in self.requirements.values():
            v.setValidator(QIntValidator(-100, 100))
        self.results = {k: QLineEdit('0') for k in self.options}
        for v in self.results.values():
            v.setValidator(QIntValidator(-100, 100))
        self.button = QLineEdit()
        self.result_text = QTextEdit()

        self.q_requirements = Layout(QHBoxLayout())
        for k, v in self.requirements.items():
            self.q_requirements.addWidget(QLabel(k))
            self.q_requirements.addWidget(v)
        self.q_results = Layout(QHBoxLayout())
        for k, v in self.results.items():
            self.q_results.addWidget(QLabel(k))
            self.q_results.addWidget(v)

        self.vbox.addWidget(self.button)
        self.vbox.addWidget(QLabel('Requirements'))
        self.vbox.addWidget(self.q_requirements)
        self.vbox.addWidget(QLabel('Results'))
        self.vbox.addWidget(self.q_results)
        self.vbox.addWidget(self.result_text)

        self.box.setLayout(self.vbox)
        self.h.addWidget(self.box)
        self.setLayout(self.h)

    def to_json(self, file, n):
        req = ",".join(self.requirements[v].text() for v in self.options[:-1])
        res = {v: self.results[v].text() for v in self.options}
        res["Requirements"] = req
        res["Name"] = self.button.text()
        res["Result"] = str(self.result_text.toPlainText())
        with open(f'./Plot/Options/{file}{n}.json', 'w',
                  encoding='utf-8') as f:
            json.dump(res, f, ensure_ascii=False)

    def clear(self):
        for i in self.requirements.values():
            i.setText('0')
        for i in self.results.values():
            i.setText('0')
        self.button.setText('')
        self.result_text.setText('')

    def init(self, dictionary):
        for l, k in zip(self.options[:-1],
                        dictionary['Requirements'].split(',')):
            self.requirements[l].setText(k)
        for t in self.options:
            a = dictionary[t] if t in dictionary.keys() else '0'
            self.results[t].setText(a)
        self.button.setText(dictionary['Name'])
        self.result_text.setText(dictionary['Result'])


class MainWindow(QWidget):
    def __init__(self):
        super().__init__()
        self.setWindowTitle("PlotCreator")

        self.vbox = QVBoxLayout()
        self.menu = Menu()
        self.name = QLineEdit()
        self.main_plot = MainPlot()
        self.start_option = ZeroOption()

        self.opt = Layout(QHBoxLayout())

        self.opt1 = Option()
        self.opt2 = Option()

        self.opt.addWidget(self.opt1)
        self.opt.addWidget(self.opt2)

        self.button = QPushButton('Compile Plot')
        self.button.clicked.connect(self.compile)

        self.menu.btn1.clicked.connect(self.clear)
        self.menu.btn2.clicked.connect(self.open_file_name_dialog)

        self.vbox.addWidget(self.menu)
        self.vbox.addWidget(self.name)
        self.vbox.addWidget(self.main_plot)
        self.vbox.addWidget(self.start_option)
        self.vbox.addWidget(self.opt)
        self.vbox.addWidget(self.button)

        self.setLayout(self.vbox)

    def compile(self):
        name = self.name.text()
        location = self.main_plot.get_location()
        self.main_plot.to_json(name)
        self.start_option.to_json(name)
        self.opt1.to_json(name, 1)
        self.opt2.to_json(name, 2)
        self.update_json(name, location)

    def clear(self):
        self.main_plot.clear()
        self.start_option.clear()
        self.opt1.clear()
        self.opt2.clear()
        self.name.setText('')

    def open_file_name_dialog(self):
        options = QFileDialog.DontUseNativeDialog
        file_name, _ = QFileDialog.getOpenFileName(self,
                                                   "Choose image",
                                                   "",
                                                   "JSON (*.json)",
                                                   options=options)
        if file_name:
            directory = './Plot/'
            name = file_name[-file_name[::-1].find('/'):-5]
            print(name)
            self.name.setText(name)
            location = self.get_locations(name)
            print(location)
            for k, v in self.main_plot.locations.items():
                if k == location:
                    v.setChecked(True)
            with open(file_name) as f:
                js = json.load(f)
            self.main_plot.text.setText(js['Text'])
            self.main_plot.image_name = js['Image']
            with open(directory + f'Options/{name}0.json') as f:
                js = json.load(f)
            self.start_option.init(js)
            with open(directory + f'Options/{name}1.json') as f:
                js = json.load(f)
            self.opt1.init(js)
            with open(directory + f'Options/{name}2.json') as f:
                js = json.load(f)
            self.opt2.init(js)

    @staticmethod
    def get_locations(name):
        with open('./Plot/Config.json', 'r') as f:
            js = json.load(f)

        for k, v in js.items():
            if name in v:
                return k

    @staticmethod
    def update_json(name, location):
        with open('./Plot/Config.json', 'r') as f:
            js = json.load(f)
        for k in js.keys():
            if name in js[k]:
                js[k].remove(name)
        js[location].append(name)
        print(js, location)
        with open(f'./Plot/Config.json', 'w',
                  encoding='utf-8') as f:
            json.dump(js, f, ensure_ascii=False)

    def closeEvent(self, event):
        reply = QMessageBox.question(
                self, "Save?",
                "Are you sure you want to quit? Any unsaved work will be lost",
                QMessageBox.Ok | QMessageBox.Cancel,
                QMessageBox.Ok)
        if reply == QMessageBox.Ok:
            event.accept()
        else:
            event.ignore()
