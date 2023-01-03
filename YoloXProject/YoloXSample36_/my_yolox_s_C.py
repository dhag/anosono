#!/usr/bin/env python3
# -*- coding:utf-8 -*-
# 
import os

from yolox.exp import Exp as MyExp


class Exp(MyExp):
    def __init__(self):
        super(Exp, self).__init__()

        self.exp_name = os.path.split(os.path.realpath(__file__))[1].split(".")[0]
        self.depth = 0.33
        self.width = 0.50
        # Define yourself dataset path
        self.data_dir = "datasets/my_data_C" # <------------ your dataset folder
        self.train_ann = "train.json"# <------------ your annotations filename for training
        self.val_ann = "val.json"# <------------ your annotations filename for validation
        self.test_ann = "test.json"# <------------ NotUse?
        self.num_classes = 1  # <------------ number of your classes
        self.max_epoch = 10 # <------------ number of your epochs.300
        self.data_num_workers = 2
        self.eval_interval = 1

